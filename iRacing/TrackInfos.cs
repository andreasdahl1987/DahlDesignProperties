using System.Collections.Generic;

namespace User.PluginSdkDemo.Categories
{

    public class TrackInfo
    {        
        public List<Tracks> trackInfo;

        public TrackInfo() {
            trackInfo = new List<Tracks>{};

              //Dirt road
            trackInfo.Add(new Tracks("atlanta rallycross long", 2, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));
            trackInfo.Add(new Tracks("atlanta rallycross short", 2, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));
            trackInfo.Add(new Tracks("brandshatch rallycross", 1, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));
            trackInfo.Add(new Tracks("charlotte 2018 2019 rallycrosslong", 3, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));
            trackInfo.Add(new Tracks("daytona rallycross long", 2, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));
            trackInfo.Add(new Tracks("daytona rallycross short", 2, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));
            trackInfo.Add(new Tracks("iowa rallycross", 2, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));
            trackInfo.Add(new Tracks("lankebanen hellrx", 1, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));
            trackInfo.Add(new Tracks("lankebanen rallyshort", 1, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));
            trackInfo.Add(new Tracks("irp rallycross", 1, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));
            trackInfo.Add(new Tracks("phoenix rallycross", 1, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));
            trackInfo.Add(new Tracks("sonoma rallycross", 1, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));
            trackInfo.Add(new Tracks("wildhorse", 4, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));
            trackInfo.Add(new Tracks("wildwest full", 4, false, 0, 0, 0, 0, false, 0, 0, 0, 0, 0, 0, false, "None"));

            //Road

            trackInfo.Add(new Tracks("daytona 2011 road", 0, false, 0, 0, 0, 0, false, 0, 19.5, 0.2, 0.3, 0.3, 0.1, true, "Left"));

            trackInfo.Add(new Tracks("silverstone 2019 gp", 0, false, 0, 0, 0, 0, false, 0, 13.5, 0.4, 0.3, 0.1, 0.2, false, "Right"));
            trackInfo.Add(new Tracks("silverstone 2019 international", 0, false, 0, 0, 0, 0, false, 0, 13.5, 0.4, 0.3, 0.1, 0.2, false, "Right"));
            trackInfo.Add(new Tracks("silverstone 2019 national", 0, false, 0, 0, 0, 0, false, 0, 17.5, 0.6, 0.2, 0, 0.2, false, "Right"));

            trackInfo.Add(new Tracks("limerock 2019 gp", 0, false, 0, 0, 0, 0, false, 0, 18.5, 0.6, 0, 0.2, 0.3, false, "Right"));
            trackInfo.Add(new Tracks("limerock 2019 classic", 0, false, 0, 0, 0, 0, false, 0, 18.5, 0.6, 0, 0.2, 0.3, false, "Right"));
            trackInfo.Add(new Tracks("limerock 2019 chicanes", 0, false, 0, 0, 0, 0, false, 0, 18.5, 0.55, 0, 0.2, 0.3, false, "Right"));
            trackInfo.Add(new Tracks("limerock 2019 chicanewestb", 0, false, 0, 0, 0, 0, false, 0, 18.5, 0.55, 0, 0.2, 0.3, false, "Right"));

            trackInfo.Add(new Tracks("longbeach", 0, true, 0.2, 0.05, 0.4, 0.05, false, 0, 20, 0, 0, 0, 0, false, "Right")); //PIT CREW INDY || base || other
            trackInfo.Add(new Tracks("okayama full", 0, false, 0, 0, 0, 0, true, 0.0035, 19, 0.2, 0.2, 0.2, 0.3, false, "Right")); //base || other
            trackInfo.Add(new Tracks("bathurst", 0, false, 0, 0, 0, 0, false, 0, 19.5, 0.7, 0.2, 0.0, 0.2, false, "Left"));
            trackInfo.Add(new Tracks("snetterton 300", 0, false, 0, 0, 0, 0, true, 0.005, 20, 0, 0, 0, 0, false, "Right")); //base || other
            trackInfo.Add(new Tracks("virginia east", 0, false, 0, 0, 0, 0, true, 0.005, 20, 0, 0, 0, 0, false, "Right")); //base || other            
        }

        public Tracks GetTrackByID(string id)
        {
            return trackInfo.Find(x => x.Id == id);
        }
    }
}