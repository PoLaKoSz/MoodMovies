# MoodMovies

Open Source application built for windows using C# and XAML. Search for movies based on your mood as well as a ton of other search and filtering options. It is built to be intuitive with the purpose of saving you from scrolling through never ending lists of movie suggestions. Targetting specific actors whilst selecting your mood and filtering by year are amongst the many awesome features you will find.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

All you need is Visual Studio. The project does have a few dependencies which have been sourced through Nuget. They should all be included in the solution when you clone the repo. If not just hit restore Nuget packages and you are ready to go with one exception.
CefSharp may show some errors regarding CPU targets and output folder paths. If you do have any issues follow this tutorial 
[How to use CefSharp](https://www.youtube.com/watch?v=fOzBVy-sDbM) and you will be up and running in no time.
 
List of Dependencies
```
-Caliburn Micro
-Material Xaml Toolkit
-NewtonSoft
-CefSharp
```

## Deployment

Deployment details still to come.

## Contributing

Currently I am not accepting pull requests, however First-Time contributors are welcome to tackle an issue. Feel free to fork the repo if you wish to go off on your own.

Exception

## Authors

* **Tony Karalis** - *Design and Implementation* - [tonykaralis](https://github.com/tonykaralis)

## License

This project is licensed under the GNU GENERAL PUBLIC LICENSE Version 3, 29 June 2007 - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

* Material Xaml Toolkit creaters for developing such an amazing toolkit and making it open source.
* Caliburn Micro for an awesome library that makes MVVM a little easier to manage.
* CefSharp for an awesome chromium browser.
* NewtonSoft for making Json easier to handle.
