#region Purpose, Credits & Terms of Use

/*
 * 
 * PURPOSE
 * 
 * MP3Player class provides basic functionality for playing MP3 files.
 * Aside from the various methods, it implements events which notify their subscribers for opening files, pausing, etc.
 * This class boosts your application's performance because it allows you to handle subscribed events as they fire instead
 * of having to check player status information at regular intervals.
 * 
 * Exceptions are also provided in the form of events since these are probably not severe and the application just needs
 * to be notified of these failures on the fly...
 * 
 * Class has been created in 2006 and revised in 2012.
 * 
 * PS: This source will only work on MS Windows, since it uses the MCI (Media Control Interface) integrated into this OS.
 * Sorry Gnu and Mono fans! I (Krasimir) hope to have enough time soon to get busy working on a similar class for these engines...
 * 
 * This class inherits from the Form class in order to receive certain notifications (such as end of song). If you are using this
 * class in a Class Library, ensure to make a reference to System.Windows.Forms
 * 
 * 
 * CREDITS
 * 
 * Krasimir kRAZY Kalinov - The main developer of this class, contributing a huge majority of the code provided herein.
 *          If you have questions, suggestions or just need to make your opinion heard, my email is krazymir@gmail.com
 * 
 * Justin C. (aka. Trapper) - Developed, revised and incorporated ideas, suggestions and other features. Thanks to all
 *          those that contributed to this class. I can be reached on trapper-hell@hotmail.com
 * 
 * 
 * ACCEPTANCE OF TERMS
 * 
 * MP3Player is provided "as is" and without warranty. The class has been developed in good faith and in hopes of providing
 * a simple wrapper to some of the basic MCI MP3 playing functions used - however, the developer(s) - identified in the
 * Credits section - accept no responsibility and cannot be held liable for any loss or damage that use of this class may incur.
 * 
 * By making use of this class you agree that you will be using this at your own risk and responsible for any risks that
 * may be introduced by the use of this class.
 * 
 * 
 * PERMITTED USE
 * 
 * The code provided herein may be modified, adjusted, shared and / or redistributed, provided that the Credits and Terms of Use
 * section are included and unchanged.
 * 
 * 
 * LIMITATION OF LIABILITY
 * 
 * IN NO EVENT WILL THE DEVELOPER(S) - IDENTIFIED IN THE CREDITS SECTION - BE LIABLE TO YOU FOR ANY LOST PROFITS, LOST SAVINGS
 * OR INCIDENTAL, INDIRECT, SPECIAL OR CONSEQUENTIAL DAMAGES, ARISING OUT OF YOUR USE OR INABILITY TO USE THE CLASS OR THE
 * BREACH OF THIS AGREEMENT, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGES. SOME STATES DO NOT ALLOW THE LIMITATION OR
 * EXCLUSION OF LIABILITY FOR INCIDENTAL OR CONSEQUENTIAL DAMAGES SO THE ABOVE LIMITATION OR EXCLUSION MAY NOT APPLY TO YOU.
 * 
 */

#endregion

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace KrasimirTrapper
{
    public enum MCINotify
    {
        Success = 0x01,
        Superseded = 0x02,
        Aborted = 0x04,
        Failure = 0x08
    }

    public class MP3Player : Form
    {
        private string Pcommand, FName, Phandle;
        private bool Opened, Playing, Paused, Looping, MutedAll, MutedLeft, MutedRight;
        private const int MM_MCINOTIFY = 0x03b9;
        private int Err, aVolume, bVolume, lVolume, pSpeed, rVolume, tVolume, VolBalance;
        private ulong Lng;

        [DllImport("winmm.dll")]
        private static extern int mciSendString(string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case MM_MCINOTIFY:
                    if (m.WParam.ToInt32() == 1)
                    {
                        Stop();
                        OnSongEnd(new SongEndEventArgs());
                    }
                    break;
                case (int)MCINotify.Aborted:
                    OnOtherEvent(new OtherEventArgs(MCINotify.Aborted));
                    break;
                case (int)MCINotify.Failure:
                    OnOtherEvent(new OtherEventArgs(MCINotify.Failure));
                    break;
                case (int)MCINotify.Success:
                    OnOtherEvent(new OtherEventArgs(MCINotify.Success));
                    break;
                case (int)MCINotify.Superseded:
                    OnOtherEvent(new OtherEventArgs(MCINotify.Superseded));
                    break;
            }

            base.WndProc(ref m);
        }

        public MP3Player()
        {
            Opened = false;
            Pcommand = string.Empty;
            FName = string.Empty;
            Playing = false;
            Paused = false;
            Looping = false;
            MutedAll = MutedLeft = MutedRight = false;
            rVolume = lVolume = aVolume = tVolume = bVolume = 1000;
            pSpeed = 1000;
            Lng = 0;
            VolBalance = 0;
            Err = 0;
            Phandle = "MP3Player";
        }

        public MP3Player(string handle)
        {
            Opened = false;
            Pcommand = string.Empty;
            FName = string.Empty;
            Playing = false;
            Paused = false;
            Looping = false;
            MutedAll = MutedLeft = MutedRight = false;
            rVolume = lVolume = aVolume = tVolume = bVolume = 1000;
            pSpeed = 1000;
            Lng = 0;
            VolBalance = 0;
            Err = 0;
            Phandle = handle;
        }

        #region Volume

        public int Balance
        {
            get
            {
                return VolBalance;
            }
            set
            {
                if (Opened && (value >= -1000 && value <= 1000))
                {
                    VolBalance = value;
                    double vPct = Convert.ToDouble(aVolume) / 1000.0;

                    if (value < 0)
                    {
                        Pcommand = string.Format("setaudio {0} left volume to {1:#}", Phandle, aVolume);
                        if ((Err = mciSendString(Pcommand, null, 0, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                        Pcommand = string.Format("setaudio {0} right volume to {1:#}", Phandle, (1000 + value) * vPct);
                        if ((Err = mciSendString(Pcommand, null, 0, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                    }
                    else
                    {
                        Pcommand = string.Format("setaudio {0} right volume to {1:#}", Phandle, aVolume);
                        if ((Err = mciSendString(Pcommand, null, 0, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                        Pcommand = string.Format("setaudio {0} left volume to {1:#}", Phandle, (1000 - value) * vPct);
                        if ((Err = mciSendString(Pcommand, null, 0, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                    }
                }
            }
        }

        public bool MuteAll
        {
            get
            {
                return MutedAll;
            }
            set
            {
                MutedAll = value;
                if (MutedAll)
                {
                    Pcommand = String.Format("setaudio {0} off", Phandle);
                    if ((Err = mciSendString(Pcommand, null, 0, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                }
                else
                {
                    Pcommand = String.Format("setaudio {0} on", Phandle);
                    if ((Err = mciSendString(Pcommand, null, 0, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                }
            }

        }

        public bool MuteLeft
        {
            get
            {
                return MutedLeft;
            }
            set
            {
                MutedLeft = value;
                if (MutedLeft)
                {
                    Pcommand = String.Format("setaudio {0} left off", Phandle);
                    if ((Err = mciSendString(Pcommand, null, 0, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                }
                else
                {
                    Pcommand = String.Format("setaudio {0} left on", Phandle);
                    if ((Err = mciSendString(Pcommand, null, 0, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                }
            }

        }

        public bool MuteRight
        {
            get
            {
                return MutedRight;
            }
            set
            {
                MutedRight = value;
                if (MutedRight)
                {
                    Pcommand = String.Format("setaudio {0} right off", Phandle);
                    if ((Err = mciSendString(Pcommand, null, 0, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                }
                else
                {
                    Pcommand = String.Format("setaudio {0} right on", Phandle);
                    if ((Err = mciSendString(Pcommand, null, 0, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                }
            }

        }

        public int VolumeAll
        {
            get
            {
                return aVolume;
            }
            set
            {
                if (Opened && (value >= 0 && value <= 1000))
                {
                    aVolume = value;
                    Pcommand = String.Format("setaudio {0} volume to {1}", Phandle, aVolume);
                    if ((Err = mciSendString(Pcommand, null, 0, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                }
            }
        }

        public int VolumeBass
        {
            get
            {
                return bVolume;
            }
            set
            {
                if (Opened && (value >= 0 && value <= 1000))
                {
                    bVolume = value;
                    Pcommand = String.Format("setaudio {0} bass to {1}", Phandle, bVolume);
                    if ((Err = mciSendString(Pcommand, null, 0, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                }
            }
        }

        public int VolumeLeft
        {
            get
            {
                return lVolume;
            }
            set
            {
                if (Opened && (value >= 0 && value <= 1000))
                {
                    lVolume = value;
                    Pcommand = String.Format("setaudio {0} left volume to {1}", Phandle, lVolume);
                    if ((Err = mciSendString(Pcommand, null, 0, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                }
            }
        }

        public int VolumeRight
        {
            get
            {
                return rVolume;
            }
            set
            {
                if (Opened && (value >= 0 && value <= 1000))
                {
                    rVolume = value;
                    Pcommand = String.Format("setaudio {0} right volume to {1}", Phandle, rVolume);
                    if ((Err = mciSendString(Pcommand, null, 0, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                }
            }
        }

        public int VolumeTreble
        {
            get
            {
                return tVolume;
            }
            set
            {
                if (Opened && (value >= 0 && value <= 1000))
                {
                    tVolume = value;
                    Pcommand = String.Format("setaudio {0} treble to {1}", Phandle, tVolume);
                    if ((Err = mciSendString(Pcommand, null, 0, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                }
            }
        }

        #endregion

        #region Properties

        public ulong AudioLength
        {
            get
            {
                if (Opened)
                    return Lng;
                else
                    return 0;
            }
        }

        public ulong CurrentPosition
        {
            get
            {
                if (Opened && Playing)
                {
                    StringBuilder s = new StringBuilder(128);
                    Pcommand = String.Format("status {0} position", Phandle);
                    if ((Err = mciSendString(Pcommand, s, 128, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                    ulong position = Convert.ToUInt64(s.ToString());
                    return position;
                }
                else
                    return 0;
            }
        }

        public string FileName
        {
            get
            {
                return FName;
            }
        }

        public string PHandle
        {
            get
            {
                return Phandle;
            }
        }

        public bool IsOpened
        {
            get
            {
                return Opened;
            }
        }

        public bool IsPlaying
        {
            get
            {
                return Playing;
            }
        }

        public bool IsPaused
        {
            get
            {
                return Paused;
            }
        }

        public bool IsLooping
        {
            get
            {
                return Looping;
            }
            set
            {
                Looping = value;
                if (Opened && Playing && !Paused)
                {
                    if (Looping)
                    {
                        Pcommand = String.Format("play {0} notify repeat", Phandle);
                        if ((Err = mciSendString(Pcommand, null, 0, this.Handle)) != 0) OnError(new ErrorEventArgs(Err));
                    }
                    else
                    {
                        Pcommand = String.Format("play {0} notify", Phandle);
                        if ((Err = mciSendString(Pcommand, null, 0, this.Handle)) != 0) OnError(new ErrorEventArgs(Err));
                    }
                }
            }
        }

        public int Speed
        {
            get
            {
                return pSpeed;
            }
            set
            {
                if (value >= 3 && value <= 4353)
                {
                    pSpeed = value;

                    Pcommand = String.Format("set {0} speed {1}", Phandle, pSpeed);
                    if ((Err = mciSendString(Pcommand, null, 0, this.Handle)) != 0) OnError(new ErrorEventArgs(Err));
                }
            }
        }

        #endregion

        #region Main Functions

        private bool CalculateLength()
        {
            try
            {
                StringBuilder str = new StringBuilder(128);
                Pcommand = "status " + Phandle + " length";
                if ((Err = mciSendString(Pcommand, str, 128, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                Lng = Convert.ToUInt64(str.ToString());

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Close()
        {
            if (Opened)
            {
                Pcommand = String.Format("close {0}", Phandle);
                if ((Err = mciSendString(Pcommand, null, 0, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                FName = string.Empty;
                Opened = false;
                Playing = false;
                Paused = false;
                OnCloseFile(new CloseFileEventArgs());
            }
        }

        public bool Open(string sFileName)
        {
            if (!Opened)
            {
                Pcommand = String.Format("open \"" + sFileName + "\" type mpegvideo alias {0}", Phandle);
                if ((Err = mciSendString(Pcommand, null, 0, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                FName = sFileName;
//                Opened = true;
                Playing = false;
                Paused = false;
                Pcommand = String.Format("set {0} time format milliseconds", Phandle);
                if ((Err = mciSendString(Pcommand, null, 0, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                Pcommand = String.Format("set {0} seek exactly on", Phandle);
                if ((Err = mciSendString(Pcommand, null, 0, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                if (!CalculateLength())
                    return false;
                Opened = true;
                OnOpenFile(new OpenFileEventArgs(sFileName));
                return true;
            }
            else
            {
                this.Close();
                this.Open(sFileName);
                return true;
            }
        }

        public void Pause()
        {
            if (Opened)
            {
                if (!Paused)
                {
                    Paused = true;
                    Pcommand = String.Format("pause {0}", Phandle);
                    if ((Err = mciSendString(Pcommand, null, 0, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                    OnPauseFile(new PauseFileEventArgs());
                }
                //else
                //{
                //    Paused = false;
                //    Pcommand = String.Format("play {0}{1} notify", Phandle, Looping ? " repeat" : string.Empty);
                //    if ((Err = mciSendString(Pcommand, null, 0, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                //    OnPlayFile(new PlayFileEventArgs());
                //}
            }
        }

        public void Play()
        {
            if (Opened)
            {
                if (!Playing)
                {
                    Playing = true;
                    Pcommand = String.Format("play {0}{1} notify", Phandle, Looping ? " repeat" : string.Empty);
                    if ((Err = mciSendString(Pcommand, null, 0, this.Handle)) != 0) OnError(new ErrorEventArgs(Err));
                    OnPlayFile(new PlayFileEventArgs());
                }
                else
                {
                    if (Paused)
                    {
                        Paused = false;
                        Pcommand = String.Format("play {0}{1} notify", Phandle, Looping ? " repeat" : string.Empty);
                        if ((Err = mciSendString(Pcommand, null, 0, this.Handle)) != 0) OnError(new ErrorEventArgs(Err));
                        OnPlayFile(new PlayFileEventArgs());
                    }
                    //else
                    //{
                    //    Pcommand = String.Format("seek {0} to start", Phandle);
                    //    if ((Err = mciSendString(Pcommand, null, 0, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                    //    Pcommand = String.Format("play {0}{1} notify", Phandle, Looping ? " repeat" : string.Empty);
                    //    if ((Err = mciSendString(Pcommand, null, 0, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                    //    OnPlayFile(new PlayFileEventArgs());
                    //}
                }
            }
        }

        public void Seek(ulong milliseconds)
        {
            if (Opened && milliseconds <= Lng)
            {
                if (Playing)
                {
                    if (Paused)
                    {
                        Pcommand = String.Format("seek {0} to {1}", Phandle, milliseconds);
                        if ((Err = mciSendString(Pcommand, null, 0, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                    }
                    else
                    {
                        Pcommand = String.Format("seek {0} to {1}", Phandle, milliseconds);
                        if ((Err = mciSendString(Pcommand, null, 0, this.Handle)) != 0) OnError(new ErrorEventArgs(Err));
                        Pcommand = String.Format("play {0}{1} notify", Phandle, Looping ? " repeat" : string.Empty);
                        if ((Err = mciSendString(Pcommand, null, 0, this.Handle)) != 0) OnError(new ErrorEventArgs(Err));
                    }
                }
            }
        }

        public void Stop()
        {
            if (Opened && Playing)
            {
                Playing = false;
                Paused = false;
                Pcommand = String.Format("seek {0} to start", Phandle);
                if ((Err = mciSendString(Pcommand, null, 0, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                Pcommand = String.Format("stop {0}", Phandle);
                if ((Err = mciSendString(Pcommand, null, 0, IntPtr.Zero)) != 0) OnError(new ErrorEventArgs(Err));
                OnStopFile(new StopFileEventArgs());
            }
        }

        #endregion

        #region Event Arguments

        public class OpenFileEventArgs : EventArgs
        {
            public OpenFileEventArgs(string filename)
            {
                this.FileName = filename;
            }
            public readonly string FileName;
        }

        public class PlayFileEventArgs : EventArgs
        {
            public PlayFileEventArgs()
            {
            }
        }

        public class PauseFileEventArgs : EventArgs
        {
            public PauseFileEventArgs()
            {
            }
        }

        public class StopFileEventArgs : EventArgs
        {
            public StopFileEventArgs()
            {
            }
        }

        public class CloseFileEventArgs : EventArgs
        {
            public CloseFileEventArgs()
            {
            }
        }

        public class ErrorEventArgs : EventArgs
        {
            [DllImport("winmm.dll")]
            static extern bool mciGetErrorString(int errorCode, StringBuilder errorText, int errorTextSize);

            public ErrorEventArgs(int ErrorCode)
            {
                this.ErrorCode = ErrorCode;

                StringBuilder sb = new StringBuilder(256);
                mciGetErrorString(ErrorCode, sb, 256);
                this.ErrorString = sb.ToString();
            }

            public readonly int ErrorCode;
            public readonly string ErrorString;
        }

        public class SongEndEventArgs : EventArgs
        {
            public SongEndEventArgs()
            {
            }
        }

        public class OtherEventArgs : EventArgs
        {
            public OtherEventArgs(MCINotify Notification)
            {
                this.Notification = Notification;
            }

            public readonly MCINotify Notification;
        }

        #endregion

        #region Event Handlers

        public delegate void OpenFileEventHandler(Object sender, OpenFileEventArgs oea);

        public delegate void PlayFileEventHandler(Object sender, PlayFileEventArgs pea);

        public delegate void PauseFileEventHandler(Object sender, PauseFileEventArgs paea);

        public delegate void StopFileEventHandler(Object sender, StopFileEventArgs sea);

        public delegate void CloseFileEventHandler(Object sender, CloseFileEventArgs cea);

        public delegate void ErrorEventHandler(Object sender, ErrorEventArgs eea);

        public delegate void SongEndEventHandler(Object sender, SongEndEventArgs seea);

        public delegate void OtherEventHandler(Object sender, OtherEventArgs oea);

        public event OpenFileEventHandler OpenFile;

        public event PlayFileEventHandler PlayFile;

        public event PauseFileEventHandler PauseFile;

        public event StopFileEventHandler StopFile;

        public event CloseFileEventHandler CloseFile;

        public event ErrorEventHandler Error;

        public event SongEndEventHandler SongEnd;

        public event OtherEventHandler OtherEvent;

        protected virtual void OnOpenFile(OpenFileEventArgs oea)
        {
            if (OpenFile != null) OpenFile(this, oea);
        }

        protected virtual void OnPlayFile(PlayFileEventArgs pea)
        {
            if (PlayFile != null) PlayFile(this, pea);
        }

        protected virtual void OnPauseFile(PauseFileEventArgs paea)
        {
            if (PauseFile != null) PauseFile(this, paea);
        }

        protected virtual void OnStopFile(StopFileEventArgs sea)
        {
            if (StopFile != null) StopFile(this, sea);
        }

        protected virtual void OnCloseFile(CloseFileEventArgs cea)
        {
            if (CloseFile != null) CloseFile(this, cea);
        }

        protected virtual void OnError(ErrorEventArgs eea)
        {
            if (Error != null) Error(this, eea);
        }

        protected virtual void OnSongEnd(SongEndEventArgs seea)
        {
            if (SongEnd != null) SongEnd(this, seea);
        }

        protected virtual void OnOtherEvent(OtherEventArgs oea)
        {
            if (OtherEvent != null) OtherEvent(this, oea);
        }

        #endregion
    }
} 