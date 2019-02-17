namespace FrigoTab {

    public enum WindowMessages {

        ActivateApp = 0x001c,
        DisplayChange = 0x007e,
        GetIcon = 0x007f,

        User = 0x4000,
        BeginSession = User + 1,
        EndSession = User + 2

    }

}
