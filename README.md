# JsonCryption
## Property-level Encrypted JSON serialization
JsonCryption offers .NET developers the ability to encrypt/decrypt individual properties/fields during serialization using Microsoft.AspNetCore.DataProtection (encryption/decryption, key management, algorithm management, etc.)
### Motivation
Integrating encryption/decryption of specified fields/properties of C# objects at the moment of JSON serialization/deserialization should be:
- Relatively easy to use
- Powered by industry-standard cryptography best-practices

JsonCryption seeks to keep initial configuration to a minimum, and only requires decorating fields and properties to be protected with a simple attribute:
```
class Foo
{
  [Encrypt]
  public string MySecret { get; set; }
}
```

### Supported Serialization Libraries
JsonCryption supports the following libraries:
- Newtonsoft.Json (preferred)
- System.Text.Json

### Supported Types
JsonCryption should support any type serializable by the JSON serializer library used. If you spot a missing type, please let me know (or better yet, create a PR!).

### Getting Started
#### Configuration
JsonCryption depends on the Microsoft.AspNetCore.DataProtection library. Therefore, you should first ensure that your DataProtection layer is [configured properly](https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/).

Next, configuration depends on the JSON serializer used...

##### Newtonsoft.Json
The implementation for Newtonsoft.Json relies on Dependency Injection. To configure JsonCryption, you'll need to register your default JsonSerializer:
```
// pseudo code
container.Register<JsonSerializer>(() => new JsonSerializer()
  {
    ContractResolver = new JsonCryptionContractResolver(container.Resolve<IDataProtectionProvider>())
  });
```

##### System.Text.Json
TODO

#### Usage
Once configured, using JsonCryption is just a matter of decorating the properties/fields you wish to encrypt and the `EncryptAttribute` and serializing your C# objects as you normally would:
```
var myFoo = new Foo("some important value", "something very public");

class Foo
{
  [Encrypt]
  public string EncryptedString { get; }
  
  public string UnencryptedString { get; }
}

// Newtonsoft.Json
var serializer = ...

using var textWriter = ...
serializer.Serialize(textWriter, myFoo);

// System.Text.Json
JsonSerializer.Serialize(myFoo);
```

### Special Stuff
The feature set is significantly different between the different JSON serializers due to differences in their customizable APIs. As of this writing, Newtonsoft.Json generally offers a much wider array of features, which is why it's preferred.

#### Fields
Currently, only Newtonsoft.Json supports serializing and encrypting fields:
```
class FieldFoo
{
  [Encrypt]
  public string MyPublicValue;
}
```

#### Non-public Properties and Fields
Again, only Newtonsoft.Json supports this currently. The easiest way to do this is to decorate the field/property with an additional `JsonPropertyAttribute`:
```
class NonPublicFoo
{
  [Encrypt]
  [JsonProperty]
  internal string InternalProperty { get; set; }
  
  [Encrypt]
  [JsonProperty]
  protected bool ProtectedField;
  
  [Encrypt]
  [JsonProperty]
  private Guid PrivateProperty { get; set; }
}
```

### Future Plans
JsonCryption is a baby and very open to PRs and more regular contributors. Feel free to reach out if you're interested in helping.

