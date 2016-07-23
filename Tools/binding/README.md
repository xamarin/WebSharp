C# and Pepper C++ binding modules
===

This is where the binding generator source is contained.

To generate the C# bindings and the C++ PepperPlugin dll entrypoints do the following:

```shell
> cd generators
\WebSharp\Tools\binding\generators>python generator.py --wnone --range=start,end --csgen --cs-enum_prefix --pepper 
```

This will create two directories in the ```binding``` folder.

- peppersharp - This is where the C# modules are generated.
- pepper - This is where the .dll entry point bindings for the PepperPlugin are generated.
