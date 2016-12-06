# Design notes

Here I discuss design ideas and decisions. Basically WireUp hopes to make it easy to wire-up a message coming into the host, 
forwarding that on... possibly after transforming it. The transport it receives it on might be a very different transport it forwards it on to.

## Example:
Message `X` is received via an HTTP POST to the host. The host receives `X`, Transforms it to `Y`, and then places `Y` onto a RabbitMQ queue.
The processing of `Y` is handled somewhere and that process then sends a message `Z` onto queue `Q` which the host is listening on. 
`Z` is then forwarded on to HTTP endpoint `B`.

> Note that any code on this page is not necessarily production code. It could be pseudo code or just future planned or imagined code.

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

These interception points could also be used for laogging/auditing but it needs to be at startup then not on a per endpoint configuration

- message hits a route
- before mapping
- after mapping
- before sending to endpoint
- after sent to endpoint
- exception
