# Open-Panda Technology API Documentation

### *By Panda Technologies*

This API is maintaining updating, if a new API is release, it will ask for you to update the Exploit's API or asked to the Developers for Latest Panda API

[![made-with-Markdown](https://img.shields.io/badge/Made%20with-Markdown-1f425f.svg)](http://commonmark.org)

![C#](https://img.shields.io/badge/Made%20with-C%23-blue)

![Discord](https://img.shields.io/discord/761754005906653245)

[![Maintenance](https://img.shields.io/badge/Maintained%3F-yes-green.svg)](https://github.com/SkieAdmin/Panda-Respiratory/graphs/commit-activity)


---
### Call API's function
```C#
PandaModuleAPI.WeArePanda api = new PandaModuleAPI.WeArePanda();
```
---
### Inject API
```C#
        PandaModuleAPI.WeArePanda api = new PandaModuleAPI.WeArePanda();
        // Example [ api.Inject(bool isPuppyMilk = false, bool JoinToPandaDiscord = true) ]
        api.Inject(true, true);       
        
```
---
### Execute Script
```C#
            PandaModuleAPI.WeArePanda api = new PandaModuleAPI.WeArePanda();
            api.Execute("print('hellow')");
```
---
### Check if Inject or Not
```C#
            PandaModuleAPI.WeArePanda api = new PandaModuleAPI.WeArePanda();
            api..PandaInjected();
```

