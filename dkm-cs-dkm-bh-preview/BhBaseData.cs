using at.denkmal.libCsDkmglobal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace dkm_cs_dkm_bh_preview
{
    public class BhBaseData
    {
        public string BhRootFolder { get; private set; }
        public static string getOutputFolder(string bhRootFolder) { 
                return Path.Combine(bhRootFolder, "upload");         
        }
        public static string getOrigFileHandledFolder(string bhRootFolder) { 
                return Path.Combine(bhRootFolder, "erledigt");            
        }

        private List<string> _lastbhFolders;
        private List<string> ensureLastBhFolders()
        {
            if (_lastbhFolders==null)
            {
                _lastbhFolders = new List<string>();
            }
            return _lastbhFolders;
        }
        public string[] LastBhFolders
        {
            get
            {
                return ensureLastBhFolders().ToArray();
            }
            set
            {
                ensureLastBhFolders();
                if (value == null)
                {
                    return;
                }
                _lastbhFolders.Clear();
                _lastbhFolders.AddRange(value.ToList<string>());
            }
        }

        public void addBhFolder(string bhFolder)
        {
            if (!ensureLastBhFolders().Exists(fld => string.Compare(fld, bhFolder,ignoreCase:true)==0))
            {
                _lastbhFolders.Add(bhFolder);
            }
        }

        public int BelegYear { get; private set; }
        public int BelegMon { get; private set; }


        public static BhBaseData Create(string bhRootFolder, int belegYear, int belegMon)
        {
            var ret = new BhBaseData();
            ret.BhRootFolder = bhRootFolder;
            ret.BelegYear = belegYear;
            ret.BelegMon = belegMon;
            return ret;
        }


        public static string getYamlFilePath()
        {
            string folder = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(folder, "config.yaml");
        }
        public static bool tryLoadYamlFile(out BhBaseData bhbase )
        {
            bhbase = null;
            string yamlFilePath = getYamlFilePath();
            if (!File.Exists(yamlFilePath))
            {
                return false;
            }
            string yaml = File.ReadAllText(yamlFilePath);

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            bhbase = deserializer.Deserialize<BhBaseData>(yaml);
            return true;
        }

        public static void saveToYaml(BhBaseData data)
        {
            string yamlFile = getYamlFilePath();
            if (File.Exists(yamlFile))
            {
                TBackupFilesUtil.createBackup(yamlFile, 10);
            }
            var serializer = new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var yaml = serializer.Serialize(data);

            File.WriteAllText(yamlFile, yaml);
        }
    }
}
