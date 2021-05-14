# ScrumLife-Scripts
Scripts to automate tasks used for the Scrum Life show

Links:
- Extract audio from video in Python: https://www.quora.com/Is-there-any-API-for-getting-audio-from-video
- AirTable API: https://airtable.com/api (to get metadata)
- YouTube API: https://www.youtube.com/intl/fr/yt/dev/api-resources/ (to upload videos)
- Turn any website into an API: https://wrapapi.com/ | https://dashblock.com/ (because Anchor does not provide any API)

## Video Manager
[![.NET Core Desktop](https://github.com/Scrum-Life/ScrumLife-Scripts/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/Scrum-Life/ScrumLife-Scripts/actions/workflows/dotnet-desktop.yml)

Automate the process by getting data from a source (like Airtable) and use it to send video and its metadatas to some services like YouTube, AnchorFM, ....

### Configure

Configuration is done by filling the appsettings.json file.
Required parameters are :
  - YoutubeConfiguration.Username : The username of the YouTube channel which hosts the videos
  - YoutubeConfiguration.AplicationName : Application name displayed when the authorization window is displayed. This can be whatever you want.
  - YoutubeConfiguration.GoogleClientId : ClientId registered and authorized to interact with YouTubeAPI in the Google API console
  - YoutubeConfiguration.GoogleClientSecret : ClientSecret generated with API key in the Google API console
  - AmaraConfiguration.ApiKey : API key generated in the Amara account
  - AirtableConfiguration.ApiKey : API key generated in the Airtable account
  - AirtableConfiguration.DatabaseId : Database ID from where to get data
  - AirtableConfiguration.TableName : Table name from where to get data

## And then ?
TODO:
Quite a lot :-P
