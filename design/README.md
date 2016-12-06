# Design notes

Here I discuss design ideas and decisions. Basically WireUp hopes to make it easy to wire-up a message coming into the host, 
forwarding that on... possibly after transforming it. The transport it receives it on might be a very different transport it forwards it on to.

All configuration will be loaded from specified assemblies.

## Example:
Message `X` is received via an HTTP POST to the host endpoint `inbox`. The host receives `X`, Transforms it to `Y`, and then places `Y` onto a RabbitMQ queue.
The processing of `Y` is handled somewhere and that process then sends a message `Z` onto queue `Q` which the host is listening on. 
`Z` is then forwarded on to HTTP endpoint `B`.

> Note that any code on this page is not necessarily production code. It could be pseudo code or just future planned or imagined code.

In the following example no mapping is specified and the message is just forwarded on as a string.

```csharp
[WireUpHttp(In = "google/?q={q}", Out = "http://google.com/", OutAs="RabbitMQ")]
public class TestHttpSchematic
{}
```

If we used this style the example detailed earlier may look like this.

```csharp
[WireUpHttp(In = "inbox", InType = typeof(X), Out = "Q", OutType = typeof(Y), OutAs="RabbitMQ")]
[WireUpRabbitMQ(In = "Q", Out = "B", InType = typeof(Z), OutAs="HTTP")]
public class Schematic // this class is arbitrary and is purely for the attributes
{}
```

## Desired features

- Configuration based routing and mapping
- Easy extension points for logging and/or auditing messages in and/or out
- Easy declaritive coded configuration
- Deeper levels of configuration as required (possibly allowing interception points at different points in the lifecycle)
- Modular addition of transports
- Possible a default transport and endpoint configuration to route all non-handled (transport then becomes optional?)

## Transports

Transports represent 

A transport represents HOW a message is received as well as HOW it is forwarded on. So in this way there are both the 

### Planned supported transports (different packages?)

 - Http
 - NService (which itself has supported transports)
 - RabbitMQ
 - SOAP
 - Https
 
## Interception points

These interception points could also be used for logging/auditing but it needs to be at startup then not on a per endpoint configuration

- message hits a route
- before mapping
- after mapping
- before sending to endpoint
- after sent to endpoint
- exception

## Mapping

TODO: Mappings should be opinionated? Explicit? Initial thought is to provide a simple mapping interface and an adapter can be written for that. Can supply one for AutoMapper via a package.

An opinionated mapping could use something like [AutoMapper Self Config](https://github.com/dburriss/AutoMapperSelfConfig) to do mappings.

A mapping could be explicitly defined by an interface `IMap<TInput, TResult>`. A schematic class would then take a more active role.

```csharp
[WireUpHttp(In = "inbox", InType = typeof(X), Out = "Q", OutType = typeof(Y), OutAs="RabbitMQ")]
[WireUpRabbitMQ(In = "Q", Out = "B", InType = typeof(Z), OutAs="HTTP")]
public class Schematic : IMap<X, Y>
{
  public Y Map(X obj)
  {
    // transform X into Y and return instance of Y
  }
}
```

## Manual configuration

This is a level deeper than the `WireUpAttribute` type configuration. Customizing the configuration could either be providing a class and overriding a type like `WireUpHttpDefinition` where properties are set and methods like Map are overridden.
Alternatively a configuration can be loaded from a class implementing `IHaveCustomWireUpConfiguration` that returns a `WireUpHttpDefinition`.
An important element is that someone can provide packages that implement other transports.

```csharp

```

## Resources

- [Routing documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/routing)
- [Middleware documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware)
- [OWIN Specification](http://owin.org/)
- [SO question on OWIN and SOAP](http://stackoverflow.com/questions/22565488/any-way-to-get-owin-to-host-a-soap-service)
- [Good reminder on HttpClient](https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/)
- [Swagger middleware](https://github.com/domaindrivendev/Ahoy/tree/master/src/Swashbuckle.SwaggerUi/Application)
