using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace DnfResourcePreloader
{
    public enum DnfResourcePreset
    {
        kFull = 0,
        kMajor,
        kTypical,
        kRaid,
        kPvp,
        kNone,
    }

    public delegate void ProgressGagueCallbak(int value);
    public delegate void PrintLogMessageCallback(String msg);

    class FileCacheLogic
    {
        ProgressGagueCallbak progressCallback_ = null;
        PrintLogMessageCallback printLogMessageCallback_ = null;

        public void SetProgressGaugeCallback(ProgressGagueCallbak callback)
        {
            progressCallback_ = callback;
        }

        public void SetPrintfLogCallback(PrintLogMessageCallback callback)
        {
            printLogMessageCallback_ = callback;
        }

        private void ProgressGauge(int value)
        {
            if (progressCallback_ != null)
                progressCallback_(value);
        }

        private void WriteLine(String msg)
        {
            if (printLogMessageCallback_ != null)
                printLogMessageCallback_(msg);
        }

        private void PrintTimeStamp(ref DateTime startTime)
        {
            var curTime = DateTime.Now;
            var pastTime = curTime - startTime;
            WriteLine("Total progress time : " + pastTime.TotalSeconds.ToString() + "sec.");
            startTime = curTime;
        }

        public String GetDNFInstallPath()
        {
            RegistryKey reg = Registry.CurrentUser;
            reg = reg.OpenSubKey(@"Software\DNF");
            String path = String.Empty;
            if (reg != null)
            {
                var obj = reg.GetValue("Path");
                if (null != obj)
                {
                    path = Convert.ToString(obj);
                }
            }
            return path;
        }

        private void GetMatchingFilesUsePreset(String path, DnfResourcePreset preset
            , out ConcurrentBag<String> files)
        {
            files = null;
            ConcurrentBag<String> newFiles = new ConcurrentBag<String>();

            Regex rgxMajorSprite = new Regex(
                "(_character)|(_common)|(_creature)|(_interface)|(_item)|(_map)|(_monster)|(_worldmap)"
                , RegexOptions.Compiled);
            Regex rgxTypicalSprite = new Regex(
                "(_character)|(_common)|(_creature)|(_interface)|(_item)|(anton)|(beast)|(fiendwar)|(luke)|(operation)|(tayberrs)|(harlem)|(hell)"
                , RegexOptions.Compiled);
            Regex rgxTypicalContents = new Regex(
                "(character)|(common)|(anton)|(beast)|(fiendwar)|(luke)|(operation)|(tayberrs)|(harlem)|(hell)"
                , RegexOptions.Compiled);
            Regex rgxRaidContents = new Regex(
                "(anton)|(fiendwar)|(luke)"
                , RegexOptions.Compiled);
            Regex rgxPvpSprite = new Regex(
                "(_character)|(_common)|(_item)|(_pvp)"
                , RegexOptions.Compiled);
            Regex rgxRaidVideo = new Regex(
                ".+cutscene"
                , RegexOptions.Compiled);

            var dirs = Directory.GetDirectories(path);
            foreach (var dir in dirs)
            {
                var dirName = Path.GetFileName(dir).ToLower();
                var subfiles = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);
                switch (preset)
                {
                    case DnfResourcePreset.kFull:
                        {
                            if (dirName.Equals("imagepacks2")
                                || dirName.Equals("soundpacks")
                                || dirName.Equals("music")
                                || dirName.Equals("video"))
                            {
                            }
                            else
                            {
                                continue;
                            }

                            Parallel.ForEach(subfiles, (subfile) =>
                            {
                                newFiles.Add(subfile);
                            });
                        }
                        break;

                    case DnfResourcePreset.kMajor:
                        {
                            Regex rgx = null;
                            if (dirName.Equals("imagepacks2"))
                            {
                                rgx = rgxMajorSprite;
                            }
                            else if (dirName.Equals("soundpacks"))
                            {
                                rgx = null;
                            }
                            else if (dirName.Equals("music"))
                            {
                                rgx = null;
                            }
                            else if (dirName.Equals("video"))
                            {
                                rgx = rgxRaidVideo;
                            }
                            else
                            {
                                continue;
                            }

                            Parallel.ForEach(subfiles, (subfile) =>
                            {
                                if (rgx != null)
                                {
                                    var file = subfile.ToLower();
                                    if (rgx.IsMatch(file))
                                    {
                                        newFiles.Add(subfile);
                                    }
                                }
                                else
                                {
                                    newFiles.Add(subfile);
                                }
                            });
                        }
                        break;

                    case DnfResourcePreset.kTypical:
                        {
                            Regex rgx = null;
                            if (dirName.Equals("imagepacks2"))
                            {
                                rgx = rgxTypicalSprite;
                            }
                            else if (dirName.Equals("soundpacks"))
                            {
                                rgx = rgxTypicalContents;
                            }
                            else if (dirName.Equals("music"))
                            {
                                rgx = rgxTypicalContents;
                            }
                            else if (dirName.Equals("video"))
                            {
                                rgx = rgxRaidVideo;
                            }
                            else
                            {
                                continue;
                            }

                            Parallel.ForEach(subfiles, (subfile) =>
                            {
                                if (rgx != null)
                                {
                                    var file = subfile.ToLower();
                                    if (rgx.IsMatch(file))
                                    {
                                        newFiles.Add(subfile);
                                    }
                                }
                                else
                                {
                                    newFiles.Add(subfile);
                                }
                            });
                        }
                        break;

                    case DnfResourcePreset.kRaid:
                        {
                            Regex rgx = null;
                            if (dirName.Equals("imagepacks2"))
                            {
                                rgx = rgxRaidContents;
                            }
                            else if (dirName.Equals("soundpacks"))
                            {
                                rgx = rgxRaidContents;
                            }
                            else if (dirName.Equals("music"))
                            {
                                rgx = rgxRaidContents;
                            }
                            else if (dirName.Equals("video"))
                            {
                                rgx = rgxRaidVideo;
                            }
                            else
                            {
                                continue;
                            }

                            Parallel.ForEach(subfiles, (subfile) =>
                            {
                                if (rgx != null)
                                {
                                    var file = subfile.ToLower();
                                    if (rgx.IsMatch(file))
                                    {
                                        newFiles.Add(subfile);
                                    }
                                }
                                else
                                {
                                    newFiles.Add(subfile);
                                }
                            });
                        }
                        break;

                    case DnfResourcePreset.kPvp:
                        {
                            Regex rgx = null;
                            if (dirName.Equals("imagepacks2"))
                            {
                                rgx = rgxPvpSprite;
                            }
                            else if (dirName.Equals("soundpacks"))
                            {
                                rgx = null;
                            }
                            else
                            {
                                continue;
                            }

                            Parallel.ForEach(subfiles, (subfile) =>
                            {
                                if (rgx != null)
                                {
                                    var file = subfile.ToLower();
                                    if (rgx.IsMatch(file))
                                    {
                                        newFiles.Add(subfile);
                                    }
                                }
                                else
                                {
                                    newFiles.Add(subfile);
                                }
                            });
                        }
                        break;

                    default:
                        {
                            Parallel.ForEach(subfiles, (subfile) =>
                            {
                                newFiles.Add(subfile);
                            });
                        }
                        break;
                }
            }

            files = newFiles;
        }

        public void Run(String path, DnfResourcePreset preset)
        {
            var startTime = DateTime.Now;

            try
            {
                if (String.IsNullOrEmpty(path))
                {
                    WriteLine("Invalid directory path!");
                    return;
                }

                WriteLine("Now begins caching files in the path " + path);

                ConcurrentBag<String> files = null;
                GetMatchingFilesUsePreset(path, preset, out files);
                int totalCount = files.Count;
                WriteLine("Total file count : " + totalCount.ToString());

                ConcurrentBag<long> uniquePercent = new ConcurrentBag<long>();
                ConcurrentBag<long> filesSize = new ConcurrentBag<long>();
                Parallel.ForEach(files, (file) =>
                {
                    try
                    {
                        var fileSize = File.ReadAllBytes(file).Length;
                        filesSize.Add(fileSize);

                        if (fileSize % 10 == 0)
                        {
                            int curCount = filesSize.Count;
                            int percent = (int)(((float)curCount / totalCount) * 100.0f);
                            if (uniquePercent.Contains(percent) == false)
                            {
                                uniquePercent.Add(percent);
                                ProgressGauge(percent);
                                WriteLine("File cached : " + curCount + "/" + totalCount.ToString()
                                    + " (" + percent.ToString() + "%)");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(file);
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                });
                ProgressGauge(100);

                long totalSize = filesSize.AsParallel().Sum() / (1024 * 1024);
                WriteLine("Total cached file size : " + totalSize.ToString() + "MB");
                PrintTimeStamp(ref startTime);
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
            }
        }
    }
}
