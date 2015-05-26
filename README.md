ngrok.editor
============

A Windows-y way of getting a website configured in IIS Express and Windows so it can be used with ngrok.

The editor is a work in progress and I built it just to solve a problem I had.  Right now "it works on my machine".  If you find an issue, feel free to log it here on GitHub, or better yet submit a pull request.

### Prerequisites

You'll need the following in order to run this application:

- .NET 4.5.  [Get it here](http://smallestdotnet.com/).
- [ngrok](https://ngrok.com).  ngrok is free but the ngrok.editor assumes you have the paid version since, well, thats just the right thing to do and you get cool things like custom subdomains.

### Getting Started with ngrok.editor

Start by downloading the [binary install](https://github.com/devinrader/ngrok.editor/releases/tag/v0.1), source or cloning the project.

When you first run the project you'll need to tell ngrok.editor where some important files are:

![App Configuration](http://i.imgur.com/t0m8ygb.png)

Once that's done, ngrok.editor will figure out all of the IIS Express websites you have on your system and let you configure both the IIS Express binding and the Windows UrlAcl needed in order to let ngrok tunnel requests to your website:

![Site Configuration](http://i.imgur.com/CUis8hE.png)

![Run NGrok](http://i.imgur.com/4tvSe4E.png)

Once you have a tunnel running, you can also launch the request inspector right from the editor.

### Building the Source

This application is WPF built using .NET 4.5 and Visual Studio 2013.  You will need an equivalent environment to open, build and run the projecton your system.

The binary installer is built using [WiX 3.8](https://wix.codeplex.com/releases/view/115492).
