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
        public string RootFolder { get; private set; }
        public string GetOutputFolder() { 
                return Path.Combine(RootFolder, "upload");         
        }
        public string GetOrigFileHandledFolder() { 
                return Path.Combine(RootFolder, "erledigt");            
        }

        public int BelegYear { get; private set; }
        public int BelegMon { get; private set; }


        public static BhBaseData Create(string rootFolder, int belegYear, int belegMon)
        {
            var ret = new BhBaseData();
            ret.RootFolder = rootFolder;
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
