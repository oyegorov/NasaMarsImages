![Screenshot](https://i.ibb.co/fk9yffq/screenshot.jpg)

# NASA Mars Images
## General Information
Within the scope of this exercise, two applications have been developed:
- NASA Mars Images downloader, a .NET Core console application that downloads all images taken by Curiosity Rover on specific dates (provided in a text file) and stores them locally.
- ASP.NET Core web application that hosts Angular frontend, allowing the user to browse the images from the local storage.

### Assumptions
- NASA Mars Rover API allows the users to retrieve images made by Curiosity, Opportunity and Spirit rovers. For the sake of simplicity, in this exercise we only work with the Curiosity images. A trivial change is needed to include images from other rovers.
- All images for the given dates are downloaded

## Building
The solution was created in VS 2017 and the projects are targeting .NET Core 2.1.
For the UI project, Node.js is needed (10.x should work).

## Solution Structure
| Project name                                  	| Description                                                                                                                                                                                                                                                                                    	|
|-----------------------------------------------	|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------	|
| StealthMonitoring.NasaImages.Api              	| Mars Rover API client interface, implementation and corresponding models                                                                                                                                                                                                                       	|
| StealthMonitoring.Storage                     	| Storage API. IImageRepository is a storage-agnostic interface for image repository. FileSystemImageRepository is an implementation that organizes image repository  on the file system using certain conventions.                                                                              	|
| StealthMonitoring.NasaImages.Downloader       	| .NET Core console application that downloads Curiosity rover images taken on given dates and stores them locally.  The download process runs asynchronously and in parallel.   The target dates are supplied in a text file. Image repository location is configurable using appsettings.json  	|
| StealthMonitoring.NasaImages.WebApp           	| A simple ASP.NET Core SPA application that hosts Angular UI. For the image gallery, a third-party component was utilized (ngx-image-gallery)                                                                                                                                                   	|
| StealthMonitoring.NasaImages.Api.Tests        	| Mars Rover API client unit tests (NUnit)                                                                                                                                                                                                                                                       	|
| StealthMonitoring.Storage.Tests               	| Image storage unit tests (NUnit)                                                                                                                                                                                                                                                               	|
| StealthMonitoring.NasaImages.Downloader.Tests 	| Image downloader tests (NUnit)                                                                                                                                                                                                                                                                 	|

### StealthMonitoring.NasaImages.Api
Everything is pretty straightworward here.
Below is IMarsRoverApiClient interface, which contains methods to download metadata for images on specific date and retrieve the actual image payload.
~~~~
    public interface IMarsRoverApiClient
    {
        Task<byte[]> DownloadImageAsync(MarsImageInfo marsImageInfo);

        Task<MarsImageInfo[]> GetImageInfosAsync(DateTime day);
    }
~~~~
MarsRoverApiClient implementation uses RestSharp library for convenience.
NASA Mars Rover API C# model classes (internal) were auto-generated from sample JSONs provided on NASA website.

### StealthMonitoring.Storage
IImageRepository provides an interface to manage the image repository.
~~~~
    public interface IImageRepository
    {
        void Reset();

        string Add(ImageMetadata imageMetadata, byte[] imageBytes);

        ImageMetadata[] FindByDate(DateTime imageDate);

        byte[] GetContent(string id);

        DateTime[] GetDatesWithAvailableImages();
    }
~~~~

FileSystemImageRepository works with a given root folder and organizes images according to the following layout:
- Subfolder names are dates when pictures were taken
- Inside the "date" subfolders images for that date are stored, with a specific naming convention.

### StealthMonitoring.NasaImages.Downloader ###
Console application. 
Accepts a parameter with the dates file name ("dates.txt" by default).

Calls Mars Rover API and gets lists of files (metadata only) for the specified dates. Then asynchronously starts downloading the actual images and puts them into the storage. When the process is successfully finished, returns 0 (-1 if there was an error).

The dates that are invalid or cannot be parsed are ignored. For instance, the problem statement has April 31 in one of the examples, which is obviously a non-existent date.

~~~
02/27/17
June 2, 2018
Jul-13-2016
April 31, 2018 - would be ignored!
~~~

### StealthMonitoring.NasaImages.WebApp ###
The application was created using Angular template:
~~~~
dotnet new angular -o my-new-app
~~~~
It hosts ImageController that works with the same storage (configurable appsettings.json).

The UI utilizes `ngx-image-gallery` component for the gallery UI. 


## Unit Tests ##
Were implemented with NUnit.

More tests could be added for a real production application.

There are some tests marked as explicit: Mars Rover API client (they call the actual NASA endpoint), and performance tests that check the speed of downloading ~100 images sequentially vs in parallel.

## Other Things ##

### Static Code Analysis ###
.NET projects use `Microsoft.CodeAnalysis.FxCopAnalyzers`

### Performance Tests ###
There are explicit NUnit tests that measure time of parallel vs sequential image downloading.
For a real production application, I'd also consider automated performance tests using NBench or something similar (if needed).

### Docker ###
The applications are implemented using .NET Core 2.1, therefore, should be 
able to run in Linux Docker containers.

For the image gallery, a shared file system volume will have to be used.

However, it's good only for testing purposes. More efficient options for BLOB storage are available.
