using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace yt_music_upload_filemaker
{
    internal class YtAudioElement
    {
        internal FileInfo audioFile { get; set; }
        internal FileInfo audioCoverPic { get; set; }
        internal YtAudioElement()
        {

        }

        internal YtAudioElement(FileInfo audioFile, FileInfo audioCoverPic)
        {
            this.audioFile = audioFile;
            this.audioCoverPic = audioCoverPic;
        }
    }
}
