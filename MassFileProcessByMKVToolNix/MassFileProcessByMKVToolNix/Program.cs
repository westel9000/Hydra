using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MassFileProcessByMKVToolNix
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * Ha nincs egy epizódhoz .srt (vagy .idx/.sub) fájl akkor elszáll null exceptionnel. 
             * Le kell kezelni vagy az érintett videó kihagyásával,
             * vagy inkább egy rövid subtitle fájl generálásával. Például ki lehet írni a videofájl nevét. 
             */
            //string VidLibraryToPrecess = @"D:\AmazonCD5\DL\Hősök S01-S04 XviD\Hosok.S03.DVDRip.XviD.Hun-HDTV";
            //string VidLibraryToPrecess = @"d:\AmazonCD5\DL\Hősök S01-S04 XviD\Heroes.S04.HUN.BDRip.XviD-HSF";
            string VidLibraryToPrecess = @"G:\mos\The.Rain.S01.1080p.NF.WEB-DL.DD5.1.x264-SCIFI";
            string OutputVideoPattern1 = "The Rain 2018";
            string OutputVideoPattern2 = "A gyilkos eső";

            DirectoryInfo vidLibrary = new DirectoryInfo(VidLibraryToPrecess);
            FileInfo[] vidLibraryFiles = vidLibrary.GetFiles();

            List<FileInfo> vidList = vidLibraryFiles.ToList();

            var mkvVideos = from mkvfiles in vidLibraryFiles.ToList()
                            where mkvfiles.Extension == ".mkv"
                            select mkvfiles;
            var aviVideos = from mkvfiles in vidLibraryFiles.ToList()
                            where mkvfiles.Extension == ".avi"
                            select mkvfiles;
            var srtVideos = from srtfiles in vidLibraryFiles.ToList()
                            where srtfiles.Extension == ".srt" && srtfiles.Name.Contains("_utf8")
                            select srtfiles;
            var idxsubVideos = from srtfiles in vidLibraryFiles.ToList()
                               where srtfiles.Extension == ".idx"
                               select srtfiles;

            Regex regex = new Regex(@"S(?<season>\d{1,2})E(?<episode>\d{1,2})", RegexOptions.IgnoreCase);
            Regex regex2 = new Regex(@"-(?<seasonepisode>\d{3})", RegexOptions.IgnoreCase);

            List<VideoAndSubtitleUnit> videoAndSubtitleUnits = new List<VideoAndSubtitleUnit>();

            List<FileInfo> anyVideos = new List<FileInfo>();

            if (mkvVideos != null)
            {
                anyVideos = mkvVideos.ToList();
            }
            else if (aviVideos != null)
            {
                anyVideos = aviVideos.ToList();
            }

            foreach (var vidItem in anyVideos)
            {
                Console.WriteLine($"Videó: {vidItem.FullName}");
                Match match = regex.Match(vidItem.FullName);
                Match match2 = regex2.Match(vidItem.FullName);
                if (match.Success)
                {
                    string season = match.Groups["season"].Value;
                    string episode = match.Groups["episode"].Value;
                    Console.WriteLine("Season: " + season + ", Episode: " + episode);

                    VideoAndSubtitleUnit tempVideoAndSubtitle = new VideoAndSubtitleUnit();
                    tempVideoAndSubtitle.season = season;
                    tempVideoAndSubtitle.episode = episode;
                    tempVideoAndSubtitle.videoFile = vidItem;
                    videoAndSubtitleUnits.Add(tempVideoAndSubtitle);
                }
                else if (match2.Success)
                {
                    bool siker = int.TryParse(match2.Groups["seasonepisode"].Value, out int seasonepisode);
                    int season = seasonepisode / 100;
                    int episode = seasonepisode % 100;

                    VideoAndSubtitleUnit tempVideoAndSubtitleUnit = new VideoAndSubtitleUnit();
                    tempVideoAndSubtitleUnit.season = season.ToString();
                    tempVideoAndSubtitleUnit.episode = episode.ToString();
                    tempVideoAndSubtitleUnit.videoFile = vidItem;
                    videoAndSubtitleUnits.Add(tempVideoAndSubtitleUnit);
                }
            }

            IEnumerable<FileInfo> subTitles;
            if (srtVideos != null)
            {
                subTitles = srtVideos;
            }
            else
            {
                subTitles = idxsubVideos;
            }

            foreach (var subItem in subTitles)
            {
                Console.WriteLine($"Felirat: {subItem.FullName}");
                Match match = regex.Match(subItem.FullName);
                Match match2 = regex2.Match(subItem.FullName);
                if (match.Success)
                {
                    string season = match.Groups["season"].Value;
                    string episode = match.Groups["episode"].Value;
                    Console.WriteLine($"Season: {season}, episode: {episode}");
                    videoAndSubtitleUnits.Where(v => v.season == season && v.episode == episode).ToList().ForEach(v => v.subtitleFile = subItem);
                }
                else if (match2.Success)
                {
                    bool siker = int.TryParse(match2.Groups["seasonepisode"].Value, out int seasonepisode);
                    int season = seasonepisode / 100;
                    int episode = seasonepisode % 100;
                    Console.WriteLine($"Season: {season}, episode: {episode}");

                    videoAndSubtitleUnits.Where(v => v.season == season.ToString() && v.episode == episode.ToString()).ToList().ForEach(v => v.subtitleFile = subItem);
                }
            }

            foreach (var vidNsubItem in videoAndSubtitleUnits)
            {
                Console.WriteLine($"Vid: {vidNsubItem.videoFile.Name} | Sub: {((vidNsubItem.subtitleFile.Name == null) ? "NOSUBTITLE" : vidNsubItem.subtitleFile.Name)}");
            }

            Console.ReadKey();

            foreach (var vidNsubItem in videoAndSubtitleUnits)
            {
                //Console.WriteLine($"mkvmerge --output '{OutputVideoPattern1} S{vidNsubItem.season}E{vidNsubItem.episode} {OutputVideoPattern2}.mkv' --language 0:hun --language 1:hun '(' '{vidNsubItem.videoFile.Name}' ')' --language 0:hun --default-track 0:yes --forced-track 0:yes '(' '{vidNsubItem.subtitleFile.Name}' ')' --track-order 0:0,0:1,1:0"); // Hősök                                                                                                                                                                                                                                                                                                                                                                
                //Console.WriteLine($"mkvmerge --output '{OutputVideoPattern1} S{vidNsubItem.season}E{vidNsubItem.episode} {OutputVideoPattern2}.mkv' --language 0:und --language 1:hun '(' '{vidNsubItem.videoFile.Name}' ')' --language 0:hun --default-track 0:no --language 1:hun --track-name 1:forced --default-track 1:yes --forced-track 1:yes '(' '{vidNsubItem.subtitleFile.Name}' ')' --track-order 0:0,0:1,1:0,1:1");                                   

                Console.WriteLine($"mkvmerge --output '{OutputVideoPattern1} S{vidNsubItem.season}E{vidNsubItem.episode} {OutputVideoPattern2}.mkv' --language 0:eng --default-track 0:yes --language 1:dan --track-name 1:Danish --default-track 1:no --language 2:eng --track-name 2:English --default-track 2:yes --language 3:eng --track-name 3:English --default-track 3:no --forced-track 3:no '(' '{vidNsubItem.videoFile.Name}' ')' --language 0:hun --default-track 0:yes '(' '{vidNsubItem.subtitleFile.Name}' ')' --track-order 0:0,0:1,0:2,0:3,1:0"); // Eső (dán)
            }

            Console.WriteLine("done!");
            Console.ReadKey();
        }
    }
}
