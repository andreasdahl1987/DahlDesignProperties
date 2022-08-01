using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.PluginSdkDemo
{
    public enum AnimationType
    {
        Analog,
        LRThreeStep,
        Ferrari488,
        CenterFourStep,
        CenterFill,
        MclarenF1,
        LMP1,
        LMP2,
        AMGGT3,
        Porsche,
        Indycar,
        Linear,
        FormulaRenault,
        MX5,
        Vee,
        AudiR8,
        LamboGT3,
        PorscheGT3R
    }

    public enum CrewType
    {
        SingleTyre = 1,
        FrontRear = 2,
        LeftRight = 3,
        All = 4,
    }
}