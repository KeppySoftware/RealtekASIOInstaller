# RealtekASIOInstaller
Easy to use installer for the Realtek ASIO libraries

## How to install the ASIO driver
1. Download the latest Realtek driver from your motherboard manufacturer or from Realtek themselves. *(I recommend the motherboard manufacturer tbh)*
2. Find the ASIO libraries inside the Realtek driver archive. *(Usually stored in a folder called "RealtekASIO_x", where x is the ASIO version)*
3. Extract the files **RTHDASIO.DLL** and **RTHDASIO64.DLL** from the archive, and place them in the same folder as **RealtekASIOInstaller.exe**.
4. Run the installer by double-clicking **RealtekASIOInstaller.exe**. *(You can also run the installer in the CMD using the **/r** argument)*
5. Profit.

## How to uninstall the ASIO driver
1. Open a command prompt on the installer's main directory.
2. Run **RealtekASIOInstaller.exe** with the **/u** argument.
3. Profit.
