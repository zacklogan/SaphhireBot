namespace SapphireBot.Json
{
    public class JSON
    {
        public class Timer
        {
            public ulong ID { get; set; }
            public DateTime Release { get; set; }
            public ulong[] Roles { get; set; }
        }

        public static List<Timer> _root = new List<Timer>();

        public static string AddTimeBan(Timer myObject)
        {
            _root.Add(myObject);
            dynamic collectionWrapper = new
            {
                 _root
            };
            return JsonConvert.SerializeObject(collectionWrapper, Formatting.Indented);
        }

        public string Settings(IGuild guild, Settings stg)
        {
            try {
                Program._client.Guilds.ToList().ForEach(x => {
                    if (!File.Exists($@"Guilds/{x.Id}/settings.json") && x.Id.Equals(guild.Id)) {
                        Dictionary<string, Dictionary<Settings, object>> settings = new Dictionary<string, Dictionary<Settings, object>>
                        {
                            { "logchannel", "" },
                        };
                        File.WriteAllText($@"Guilds/{guild.Id}settings.json", JsonConvert.SerializeObject(welcome, Formatting.Indented));
                    }
                });
                Dictionary<string, Dictionary<Settings, object>> _json = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<Settings, object>>>(File.ReadAllText($@"Settings/settings_{guild.Id}.json"));
                return (string)_json["Settings"][stg];
            }
            catch {
                Dictionary<string, Dictionary<Settings, object>> _json0 = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<Settings, object>>>(File.ReadAllText($@"Settings/settings_{guild.Id}.json"));
                _json0["Settings"].TryAdd(stg, _bc.inWelcome.Where(x => x.Key.Equals(stg)).Select(x => x.Value).Single());
                File.WriteAllText($@"Settings/settings_{guild.Id}.json", JsonConvert.SerializeObject(_json0, Formatting.Indented));
                return _bc.inWelcome.Where(x => x.Key.Equals(stg)).Select(x => x.Value).Single().ToString();
            }
        }

        public object User_data(ulong id, User info)
        {
            Modules.BaseCommands _bc = new Modules.BaseCommands();
            try
            {
                if (!File.Exists($@"users/user_{id}.json"))
                {
                    Dictionary<string, Dictionary<User, object>> userjson = new Dictionary<string, Dictionary<User, object>>
                        {
                            { "info", { User.Shekels, 100 }, { User.Bank, 50 }, { User.Storage, 1 } }}
                        };
                    File.WriteAllText($@"users/user_{id}.json", JsonConvert.SerializeObject(userjson, Formatting.Indented));
                }
                Dictionary<string, Dictionary<User, object>> _json = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<User, object>>>(File.ReadAllText($@"users/user_{id}.json"));
                return _json["info"][info];
            }
            catch
            {
                Dictionary<string, Dictionary<User, object>> _json0 = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<User, object>>>(File.ReadAllText($@"users/user_{id}.json"));
                _json0["info"].TryAdd(info, _bc.inUser.Where(x => x.Key.Equals(info)).Select(x => x.Value).Single());
                File.WriteAllText($@"users/user_{id}.json", JsonConvert.SerializeObject(_json0, Formatting.Indented));
                return _bc.inUser.Where(x => x.Key.Equals(info)).Select(x => x.Value).Single().ToString();
            }
        }

        public void Set_User_Data(ulong id, User info, object data)
        {
            if (!DoesExistUser(id)) { CreateUser(id); }
            JObject rss = JObject.Parse(File.ReadAllText($@"users/user_{id}.json"));
            JObject welcome = (JObject)rss["info"];
            if (!welcome.TryAdd(info.ToString(), (int)data)) {
                welcome[info.ToString()] = (int)data;
            }
            File.WriteAllText($@"users/user_{id}.json", rss.ToString());
        }

        public static void DeleteJSON(IGuild guild)
        {
            Ext._vMem _vMem = new Ext._vMem();
            if (File.Exists($@"Settings/settings_{guild.Id}.json")) {
                File.Delete($@"Settings/settings_{guild.Id}.json");
            }
            if (File.Exists($@"json/jayson_{guild.Id}.json")) {
                File.Delete($@"json/jayson_{guild.Id}.json");
            }
            _vMem._run();
        }

        public static void DeleteUserData(ulong id)
        {
            if (File.Exists($@"users/user_{id}.json")) {
                File.Delete($@"users/user_{id}.json");
            }
        }

        public static void DeleteJSON(IGuild guild, ulong id)
        {
            if (File.Exists($@"json/jayson_{guild.Id}.json")) {
                try {
                    Dictionary<ulong, Dictionary<Commands, bool>> perm = JsonConvert.DeserializeObject<Dictionary<ulong, Dictionary<Commands, bool>>>(File.ReadAllText($@"json/jayson_{guild.Id}.json"));
                    perm.Remove(id);
                    File.WriteAllText($@"json/jayson_{guild.Id}.json", JsonConvert.SerializeObject(perm, Formatting.Indented));
                }
                catch {
                    throw new Exception("Can not get Dictionary from JSON file!");
                }
            }
        }

        public void SetTimeJSON(IGuild guild, IUser user, DateTime time)
        {
            Timer timer = new Timer
            {
                ID = user.Id,
                Release = time,
                Roles = ((user as SocketGuildUser).Roles).Where(x => x.Id != guild.EveryoneRole.Id).Select(y => y.Id).ToArray()
            };
            File.WriteAllText($@"Time/time_{guild.Id}.json", AddTimeBan(timer));
        }

        public bool GetTime(IGuild guild, ulong id)
        {
            Main.Program._client.Guilds.ToList().ForEach(x => {
                if (!File.Exists($@"Time/time_{x.Id}.json") && x.Id.Equals(guild.Id)) {
                    File.Create($@"Time/time_{x.Id}.json").Dispose();
                }
            });
            JObject rss = JObject.Parse(File.ReadAllText($@"Time/time_{guild.Id}.json"));
            JArray dataTable = (JArray)rss["_root"];
            for (int y = 0; y < dataTable.Count; y++) {
                if (Convert.ToUInt64((string)dataTable[y]["ID"]).Equals(id)) {
                    if (DateTime.ParseExact((string)dataTable[y]["Release"], "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture) <= DateTime.Now) {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }
            return false;
        }

        private void RemoveFields(JToken token, string[] fields)
        {
            JContainer container = token as JContainer;
            if (container == null) return;

            List<JToken> removeList = new List<JToken>();
            foreach (JToken el in container.Children()) {
                if (el is JProperty p && fields.Contains(p.Name)) {
                    removeList.Add(el);
                }
                RemoveFields(el, fields);
            }
            foreach (JToken el in removeList) {
                el.Remove();
            }
        }
    }


    public static class Extensions
    {
        public static JToken RemoveFields(this JToken token, string[] fields)
        {
            JContainer container = token as JContainer;
            if (container == null) return token;

            List<JToken> removeList = new List<JToken>();
            foreach (JToken el in container.Children()) {
                if (el is JProperty p && fields.Contains(p.Name)) {
                    removeList.Add(el);
                }
                el.RemoveFields(fields);
            }

            foreach (JToken el in removeList) {
                el.Remove();
            }

            return token;
        }
    }
}
