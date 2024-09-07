using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Tris
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static int selezione = 0;
        public static int selezione1 = 4;
        public static int tmp = 0;
        public static P1vsP2 p1VSp2;
        public static P1vsCPU p1VSCPU;
        public static P1vsAI p1VSAI;
    }
}
