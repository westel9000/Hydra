using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace yt_music_upload_filemaker
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

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string VidLibraryToPrecess = @"G:\Virtual Machines\Ubuntu Destop\Gshared\COL\GG music\Cyberpunkmusic";
            //string OutputVideoPattern1 = "The Rain 2018";
            //string OutputVideoPattern2 = "A gyilkos eső";

            DirectoryInfo vidLibrary = new DirectoryInfo(VidLibraryToPrecess);
            FileInfo[] audioLibraryFiles = vidLibrary.GetFiles();

            List<FileInfo> vidList = audioLibraryFiles.ToList();

            var mkaMusics = from mkafiles in audioLibraryFiles.ToList()
                            where mkafiles.Extension == ".mka"
                            select mkafiles;
            var webmMusics = from webmfiles in audioLibraryFiles.ToList()
                            where webmfiles.Extension == ".webm"
                            select webmfiles;
            var webpCovers = from webpfiles in audioLibraryFiles.ToList()
                            where webpfiles.Extension == ".webp"
                            select webpfiles;
            var jpgCovers = from jpgfiles in audioLibraryFiles.ToList()
                               where jpgfiles.Extension == ".idx"
                               select jpgfiles;

            Regex regex = new Regex(@"S(?<season>\d{1,2})E(?<episode>\d{1,2})", RegexOptions.IgnoreCase);
            Regex regex2 = new Regex(@"-(?<seasonepisode>\d{3})", RegexOptions.IgnoreCase);

            List<YtAudioElement> ytAudioElements = new List<YtAudioElement>();

            List<FileInfo> anyAudio = new List<FileInfo>();

            int countmka = mkaMusics.Count();
            int countwebm = webmMusics.Count();

            if (mkaMusics.Count() > 0)
            {
                anyAudio = mkaMusics.ToList();
            }
            else if (webmMusics.Count() > 0)
            {
                anyAudio = webmMusics.ToList();
            }

            foreach (var audioItem in anyAudio)
            {
                Console.WriteLine($"Audio: {audioItem.FullName}");
                YtAudioElement ytAudioElement = new YtAudioElement();
                ytAudioElement.audioFile = audioItem;
                ytAudioElement.audioCoverPic = webpCovers.Where(w => Path.GetFileNameWithoutExtension(w.Name) == Path.GetFileNameWithoutExtension(audioItem.FullName) ).FirstOrDefault();
                if (ytAudioElement.audioCoverPic == null)
                {
                    ytAudioElement.audioCoverPic = jpgCovers.Where(w => Path.GetFileNameWithoutExtension(w.Name) == audioItem.FullName).FirstOrDefault();
                }
                ytAudioElements.Add(ytAudioElement);
            }
            foreach (var audioNcoverItem in ytAudioElements)
            {
                Console.WriteLine($"Vid: {audioNcoverItem.audioFile.Name} | Cover pic: {audioNcoverItem.audioCoverPic.Name}");
            }

            Console.ReadKey();

            List<string> writeToFile = new List<string>();
            foreach (var audioNcoverpicItem in ytAudioElements)
            {
                //Console.WriteLine($"mkvmerge --output '{OutputVideoPattern1} S{vidNsubItem.season}E{vidNsubItem.episode} {OutputVideoPattern2}.mkv' --language 0:hun --language 1:hun '(' '{vidNsubItem.videoFile.Name}' ')' --language 0:hun --default-track 0:yes --forced-track 0:yes '(' '{vidNsubItem.subtitleFile.Name}' ')' --track-order 0:0,0:1,1:0"); // Hősök                                                                                                                                                                                                                                                                                                                                                                
                //Console.WriteLine($"mkvmerge --output '{OutputVideoPattern1} S{vidNsubItem.season}E{vidNsubItem.episode} {OutputVideoPattern2}.mkv' --language 0:und --language 1:hun '(' '{vidNsubItem.videoFile.Name}' ')' --language 0:hun --default-track 0:no --language 1:hun --track-name 1:forced --default-track 1:yes --forced-track 1:yes '(' '{vidNsubItem.subtitleFile.Name}' ')' --track-order 0:0,0:1,1:0,1:1");                                   

                byte[] bytes = Encoding.Default.GetBytes(audioNcoverpicItem.audioFile.Name);
                string audioFileNameUft8 = Encoding.UTF8.GetString(bytes);

                bytes = Encoding.Default.GetBytes(audioNcoverpicItem.audioCoverPic.Name);
                string coverPicUtf8 = Encoding.UTF8.GetString(bytes);

                Console.WriteLine("rm cover.jpg");
                writeToFile.Add("rm cover.jpg");
                Console.WriteLine("sleep 1");
                writeToFile.Add("sleep 10");
                Console.WriteLine($"convert \"{coverPicUtf8}\" cover.jpg");
                writeToFile.Add($"convert \"{coverPicUtf8}\" cover.jpg");
                Console.WriteLine("sleep 1");
                writeToFile.Add("sleep 10");

                Console.WriteLine($"mkvmerge --output \"{audioFileNameUft8}.mka\" \"{audioFileNameUft8}\" --attach-file \"cover.jpg\" "); 
                writeToFile.Add($"mkvmerge --output \"{audioFileNameUft8}.mka\" \"{audioFileNameUft8}\" --attach-file \"cover.jpg\" ");
            }

            File.WriteAllLines("outexecit.txt", writeToFile);
            Console.WriteLine("done!");
            Console.ReadKey();
        }
    }
}
