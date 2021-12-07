using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            string VidLibraryToPrecess = @"G:\mos\The.Rain.S01.1080p.NF.WEB-DL.DD5.1.x264-SCIFI";
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

            if (mkaMusics != null)
            {
                anyAudio = mkaMusics.ToList();
            }
            else if (webmMusics != null)
            {
                anyAudio = webmMusics.ToList();
            }

            foreach (var audioItem in anyAudio)
            {
                Console.WriteLine($"Audio: {audioItem.FullName}");
                YtAudioElement ytAudioElement = new YtAudioElement();
                ytAudioElement.audioFile = audioItem;
                ytAudioElement.audioCoverPic = webpCovers.Where(w => Path.GetFileNameWithoutExtension(w.Name) == audioItem.FullName).FirstOrDefault();
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

            foreach (var audioNcoverpicItem in ytAudioElements)
            {
                //Console.WriteLine($"mkvmerge --output '{OutputVideoPattern1} S{vidNsubItem.season}E{vidNsubItem.episode} {OutputVideoPattern2}.mkv' --language 0:hun --language 1:hun '(' '{vidNsubItem.videoFile.Name}' ')' --language 0:hun --default-track 0:yes --forced-track 0:yes '(' '{vidNsubItem.subtitleFile.Name}' ')' --track-order 0:0,0:1,1:0"); // Hősök                                                                                                                                                                                                                                                                                                                                                                
                //Console.WriteLine($"mkvmerge --output '{OutputVideoPattern1} S{vidNsubItem.season}E{vidNsubItem.episode} {OutputVideoPattern2}.mkv' --language 0:und --language 1:hun '(' '{vidNsubItem.videoFile.Name}' ')' --language 0:hun --default-track 0:no --language 1:hun --track-name 1:forced --default-track 1:yes --forced-track 1:yes '(' '{vidNsubItem.subtitleFile.Name}' ')' --track-order 0:0,0:1,1:0,1:1");                                   

                Console.WriteLine($"mkvmerge --output '{audioNcoverpicItem.audioFile.Name}.mkv' --attach-file file-name '{audioNcoverpicItem.audioFile.Name}' --attachment-name name 'cover.jpg' "); 
            }

            Console.WriteLine("done!");
            Console.ReadKey();
        }
    }
}
