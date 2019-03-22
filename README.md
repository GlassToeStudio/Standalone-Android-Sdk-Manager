# Standalone-Android-Sdk-Manager

* If you have been using the Sdk Manager that is included with Android Studio, this should look and feel pretty familiar. :D

<p align="center">
  <br>
  <img src="https://i.imgur.com/zj5ccIs.png">
</p>

## Getting Started
---
   * Download and Install the latest [Release](https://github.com/GlassToeStudio/Standalone-Android-Sdk-Manager/releases)
   * Unzip the folder contents.
   * Run setup.exe

   *you may get a warning, because the cert used to sign is not widely known.*

   *If this is too risky for you, there is always the option to clone the repo, and build the exe on your own system :D*

---
## Prerequisites
---

__This Program requires the Android SDK Command Line Tools and the Java JDK__
### ★ Already have a valid Sdk installed and Java JDK?
* You are done :D

---

#### ★ Don't have a valid JDK installed?

 * __Go download [Java JDK](https://www.oracle.com/technetwork/java/javase/downloads/jdk8-downloads-2133151.html) and install it. (For Windows)__

    * After installation add Java to your `PATH`
    * More information about adding Java to `PATH` can be found [here](https://javatutorial.net/set-java-home-windows-10)

#### ★ Don't have a valid Sdk installed?

  * __Go download [Android Sdk Command Line Tools](https://developer.android.com/studio#downloads). (For windows)__

    * Create a root `Sdk` folder somewhere that is easy to find `C:\\` is a good place.
    * Extract the zip file to that root `Sdk` folder.
    * Folder structure should be `C:\\Sdk\tools` (if you chose `C:\\`)
    * Folder structure is important for this to work, since you will need to navigate to the root `Sdk` folder within the application, It will then look in `\tools\bin\` for the sdkmanager.bat file that is included with the command-line tools.
 
---

### Notes:
* I named the root folder `Sdk`, the name does not matter. Additionally, this folder can be inside any other folder. The important part is that you know which is the root. 
* A common structure is `C:\\Android\Sdk\` or `C:\\Android-Sdk\` as the root folder name.
* It is preferable (but not necessary) that `C:\\Sdk\tools` has been added to your `PATH`.
* More info about adding to your `PATH` can be found [here](https://www.androidcentral.com/installing-android-sdk-windows-mac-and-linux-tutorial) 


* If contributing, be sure to have `git` installed.
---

## Usage
---

* After installing, and upon the first run, you will need to navigate to your Sdk `root` folder.
* If you had previously used Android Studio to install the Sdk, the default location is:
  * `%LOCALAPPDATA%\Android\Sdk` 
  * aka: `C:\\Users\UserName\AppData\Local\Android\Sdk`
* The program will automatically populate the Sdk field with this info, if it is not where your Sdk is installed, you will need to manually navigate to your Sdk location.
* You should only need to do this once, as the program will save the location. 
* Now you can use the UI to manage your Sdk. 
  * Install new packages and tools
  * Uninstall old packages
  * Update current packages
  * etc.

---
## Contributing
---
This project is open source and controlled under the [MIT License](LICENSE) 

Want to contribute?

See the [Contributing](CONTRIBUTING.md) guidelines!

Please be sure to check the [Code of Conduct ](CODE_OF_CONDUCT.md) as well!