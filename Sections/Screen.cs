namespace DahlDesign.Plugin.Categories
{
    public class Screen
    {
        public int screenID { get; set; }
        public int maxScreens;

        public string[] screenNames;

        public Screen(string[] screenNames, int maxscreens, int currentScreen = 1)
        {
            this.maxScreens =maxscreens;
            this.screenNames = screenNames;
            screenID = currentScreen;
        }

        public void Next()
        {
            if (++screenID > maxScreens) screenID = 1;
        }

        public void Previous()
        {            
            if (--screenID < 1) screenID = maxScreens;
        }

        public void First()
        { screenID = 1; }

        public string GetScreenName()
        {
            return screenNames[screenID+1];
        }

        public void SetScreenByName(string screenName)
        {
            screenID = System.Array.IndexOf(screenNames, screenName); 
        }

    }
}