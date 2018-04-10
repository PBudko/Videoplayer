using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_C_sharp_45_wpf.Model
{
  static class PlayList
    {
        static ObservableCollection<Element> playList = new ObservableCollection<Element>();

        public static ObservableCollection<Element> Playlist
        {
            get { return playList; }
        }        

        public static bool IsEmpty()
        {
            if (Playlist.Count == 0)
                return true;
            return false;
        }

        public static void ADD(string NameFile, string PathFile)
        {
            playList.Add(new Element(NameFile, PathFile));
        }
    }
}
