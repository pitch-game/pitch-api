# Pitch API
API for Pitch in a microservices architecture on containers

![GitHub last commit](https://img.shields.io/github/last-commit/pitch-game/pitch-api.svg)
![GitHub issues](https://img.shields.io/github/issues/pitch-game/pitch-api.svg)
![Mozilla HTTP Observatory Grade](https://img.shields.io/mozilla-observatory/grade/api.pitch-game.io.svg)
![GitHub repo size](https://img.shields.io/github/repo-size/pitch-game/pitch-api.svg)
![GitHub](https://img.shields.io/github/license/pitch-game/pitch-api.svg)

|    Service   |  Build Status  |    Deloyment Status   |   Tests   |   Code Coverage
|     :---:    |     :---:      |     :---:      |     :---:      |     :---:      |  
| [Pitch.Gateway.Api](/src/api-gateway)   | [![Build Status](https://dev.azure.com/pitch-game/Pitch%20API/_apis/build/status/Pitch.Gateway.Api?branchName=master)](https://dev.azure.com/pitch-game/Pitch%20API/_build/latest?definitionId=14&branchName=master) | [![Deployment Status](https://vsrm.dev.azure.com/pitch-game/_apis/public/Release/badge/01e573de-5dd1-4889-95d0-1578288493e2/12/12)](https://dev.azure.com/pitch-game/Pitch%20API/_release?view=mine&definitionId=12)
| [Pitch.Card.Api](/src/card)    | [![Build Status](https://dev.azure.com/pitch-game/Pitch%20API/_apis/build/status/Pitch.Card.Api?branchName=master)](https://dev.azure.com/pitch-game/Pitch%20API/_build/latest?definitionId=17&branchName=master) | [![Deployment Status](https://vsrm.dev.azure.com/pitch-game/_apis/public/Release/badge/01e573de-5dd1-4889-95d0-1578288493e2/13/13)](https://dev.azure.com/pitch-game/Pitch%20API/_release?view=mine&definitionId=13)
| [Pitch.Identity.Api](src/identity)  | [![Build Status](https://dev.azure.com/pitch-game/Pitch%20API/_apis/build/status/Pitch.Identity.Api?branchName=master)](https://dev.azure.com/pitch-game/Pitch%20API/_build/latest?definitionId=16&branchName=master)    | [![Deployment Status](https://vsrm.dev.azure.com/pitch-game/_apis/public/Release/badge/01e573de-5dd1-4889-95d0-1578288493e2/14/14)](https://dev.azure.com/pitch-game/Pitch%20API/_release?view=mine&definitionId=14)  
| [Pitch.Player.Api](/src/player)     | [![Build Status](https://dev.azure.com/pitch-game/Pitch%20API/_apis/build/status/Pitch.Player.Api?branchName=master)](https://dev.azure.com/pitch-game/Pitch%20API/_build/latest?definitionId=15&branchName=master) | [![Deployment Status](https://vsrm.dev.azure.com/pitch-game/_apis/public/Release/badge/01e573de-5dd1-4889-95d0-1578288493e2/15/15)](https://dev.azure.com/pitch-game/Pitch%20API/_release?view=mine&definitionId=15)  
[Pitch.Squad.Api](/src/squad)       | [![Build Status](https://dev.azure.com/pitch-game/Pitch%20API/_apis/build/status/Pitch.Squad.Api?branchName=master)](https://dev.azure.com/pitch-game/Pitch%20API/_build/latest?definitionId=12&branchName=master) | [![Deployment Status](https://vsrm.dev.azure.com/pitch-game/_apis/public/Release/badge/01e573de-5dd1-4889-95d0-1578288493e2/10/10)](https://dev.azure.com/pitch-game/Pitch%20API/_release?view=mine&definitionId=10)  
|  [Pitch.Store.Api](/src/store)     | [![Build Status](https://dev.azure.com/pitch-game/Pitch%20API/_apis/build/status/Pitch.Store.Api?branchName=master)](https://dev.azure.com/pitch-game/Pitch%20API/_build/latest?definitionId=13&branchName=master) | [![Deployment Status](https://vsrm.dev.azure.com/pitch-game/_apis/public/Release/badge/01e573de-5dd1-4889-95d0-1578288493e2/11/11)](https://dev.azure.com/pitch-game/Pitch%20API/_release?view=mine&definitionId=11)  
| [Pitch.User.Api](/src/user)        | [![Build Status](https://dev.azure.com/pitch-game/Pitch%20API/_apis/build/status/Pitch.User.Api?branchName=master)](https://dev.azure.com/pitch-game/Pitch%20API/_build/latest?definitionId=18&branchName=master)  | [![Deployment Status](https://vsrm.dev.azure.com/pitch-game/_apis/public/Release/badge/01e573de-5dd1-4889-95d0-1578288493e2/17/17)](https://dev.azure.com/pitch-game/Pitch%20API/_release?view=mine&definitionId=17)  
| [Pitch.Match.Api](/src/match)       | [![Build Status](https://dev.azure.com/pitch-game/Pitch%20API/_apis/build/status/Pitch.Match.Api?branchName=master)](https://dev.azure.com/pitch-game/Pitch%20API/_build/latest?definitionId=11&branchName=master) | [![Deployment Status](https://vsrm.dev.azure.com/pitch-game/_apis/public/Release/badge/01e573de-5dd1-4889-95d0-1578288493e2/16/16)](https://dev.azure.com/pitch-game/Pitch%20API/_release?view=mine&definitionId=16) |   ![Azure DevOps tests](https://img.shields.io/azure-devops/tests/pitch-game/Pitch%20API/11) | ![Azure DevOps coverage](https://img.shields.io/azure-devops/coverage/pitch-game/Pitch%20API/11)

## Getting Started

For instructions on how to get a copy of the project up and running on your local machine for development and testing purposes, please see the wiki article on [setting up a local development environment](../../wiki/Setting-up-a-local-development-environment).


## Deployment

To deploy Pitch to a production environment in the cloud please see either [deploying to AKS in production](../../wiki/Deploying-to-AKS-in-production) or [deploying to AWS in production](../../wiki/Deploying-to-AWS-in-production).

## Built With

* [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-2.2) - The web framework used

## Contributing

Please read [CONTRIBUTING.md](https://gist.github.com/PurpleBooth/b24679402957c63ec426) for details on our code of conduct, and the process for submitting pull requests to us.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](/tags). 

## Authors

* **Jacob** - *Initial work* - [Jcbcn](https://github.com/jcbcn)

See also the list of [contributors](/contributors) who participated in this project.

## License

This project is licensed under the GPL 3.0 License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

* [Jetbrains](https://www.jetbrains.com/) for providing an 'All Products Pack' license as part of their open source initiative.
