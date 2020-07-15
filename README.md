# Digital First Careers - Subscriptions

This project handles the subscription to an Event Grid Topic and also the receiving of Webhook Events

## Getting Started

This is a self-contained Visual Studio 2019 solution containing a number of projects (.NET Standard Class Library with associated unit test project).

The SubscriptionRegistrationBackgroundService requires a configuration section in the consuming application. An example of this is as follows:

```
  "SubscriptionSettings": {
    "Endpoint": "https://dfc-dev-app-contactus-as.azurewebsites.net/api/webhook/ReceiveEvents",
    "SubscriptionServiceEndpoint": "https://dfc-dev-api-eventgridsubscriptions-fa.azurewebsites.net/api/Execute",
    "Filter": {
      "BeginsWith": "ATest",
      "EndsWith":  "AnotherTest",
      "IncludeEventTypes": [ "published", "unpublished", "deleted" ],
      "PropertyContainsFilters": [
        {
          "Key": "subject",
          "Values": [ "e11a1137-01ca-446a-b60f-0de5ad5321cc", "e11a1195-801d-479b-84b6-f5e443abfb86" ]
        }
      ]
    }
  }
```

### Installing

Clone the project and open the solution in Visual Studio 2019.

## Deployments

This package deploys a NuGet package onto a NuGet feed. It is then consumed by the Shell and Comp UI child apps.


## Built With

* Microsoft Visual Studio 2019
* .Net Standard 2.1

