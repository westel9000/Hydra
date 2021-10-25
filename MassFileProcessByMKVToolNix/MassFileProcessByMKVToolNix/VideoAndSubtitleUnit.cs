using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassFileProcessByMKVToolNix
{
    public class VideoAndSubtitleUnit
    {
        public FileInfo videoFile { get; set; }
        public FileInfo subtitleFile { get; set; }
        public string season { get; set; }
        public string episode { get; set; }

        public VideoAndSubtitleUnit()
        {
        }

        public VideoAndSubtitleUnit(FileInfo videoFile, FileInfo subtitleFile, string season, string episode)
        {
            this.videoFile = videoFile ?? throw new ArgumentNullException(nameof(videoFile));
            this.subtitleFile = subtitleFile ?? throw new ArgumentNullException(nameof(subtitleFile));
            this.season = season ?? throw new ArgumentNullException(nameof(season));
            this.episode = episode ?? throw new ArgumentNullException(nameof(episode));
        }
    }
}
