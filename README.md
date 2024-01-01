# Galahad
Galahad is an extensible markup language inspired by XAML.

## Galahad vs XAML

| Feature                      | Galahad                                                                                                     | XAML                                                                                                                                                       |
|------------------------------|-------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Syntax**                   | Galahad is a more compact and expressive representation of structures that in XAML might require significantly more space. | XAML is a more verbose representation of structures that in Galahad might require significantly less space.                                                |
| **ResourceDictionary**       | Galahad uses the attribute [key]="value" for defining resources.                                           | XAML employs ResourceDictionary with Resources property, where resources are defined with a Key and value, e.g., `<Style x:Key="MyStyle">...</Style>`.    |
| **Namespace attached property** | Galahad supports namespace attached properties, when you use the alias for the namespace.                | XAML does not support namespace attached properties.                                                                                                      |
| **Complex DynamicResource**  | Galahad supports complex DynamicResource, which contains FallbackValue, Converter, and ConverterParameters properties. | XAML DynamicResource only references a key.                                                                                                               |
| **Binding to IObservable**   | Galahad supports binding to IObservable.                                                                   | XAML does not support binding to IObservable.                                                                                                              |



## Galahad syntax

This how galahad looks like:
```xml
@using Galahad alias x 
@using Galahad.AbstractPresentation

<UserControl>
	<Grid RowDefinations="[*, Auto]">
		<TextBox x:Name="text" Grid.Row="0"
                   [{StyleSelector "TextBlock"}] = "{Style TextBox , FontSize = {DynamicResource "FontSize"}}"
                   [{StyleSelector "TextBlock:Focused"} = "{Style TextBox, Foreground = Green}"
                   [{StyleSelector "TextBlock:Invalid"} = "{Style TextBox, Foreground = Red}"
                   Behaviors = "[{IsEmalBehavior}]"
                   ["FontSize"] = "double(14)"
                   ["Text"] = "Hello World"
                   Text="{DynamicResource "Text"}" />
	</Grid>
</UserControl>
```

Xaml analog:
```xaml
<UserControl xmlns="https://xamlgalahad.com/abstract-presentation"
             xmlns:x="https://xamlgalahad.com">
	<Grid>
		
		<Grid.RowDefinations>
			<RowDefinition Height="*" />
			<RowDefinition Height="Auto" />
		</Grid.RowDefinations>

		<TextBlock x:Name="text" 
				   Text="{DynamicResource "Text"}">
			<TextBlock.Resources>
				<ResourceDictionary>
					<Style TargetType="TextBlock">
						<Setter Property="FontSize" Value="{DynamicResource "FontSize"}" />
					</Style>

					<Style TargetType="TextBlock" x:Key="TextBlock:Focused">
						<Setter Property="Foreground" Value="Green" />
					</Style>

					<Style TargetType="TextBlock" x:Key="TextBlock:Invalid">
						<Setter Property="Foreground" Value="Red" />
					</Style>

					<x:Double x:Key="FontSize">14</x:Double>
					<x:String x:Key="Text">Hello World</x:String>
				</ResourceDictionary>
			</TextBlock.Resources>
			<TextBlock.Behaviors>
				<IsEmailBehavior />
			</TextBlock.Behaviors>
		</TextBlock>
	</Grid>

</UserControl>
```

### Using Directives
In Galahad, the @using directive is employed to include namespaces into your document, simplifying access to various types and components defined within them. It enhances readability and reduces the verbosity often encountered in XML-based languages.

- Basic Usage: Simply use @using NamespaceName to include a namespace. This allows direct usage of types within that namespace in your markup.

- Alias Feature: You can assign an alias to a namespace using the syntax @using NamespaceName alias AliasName. This feature is particularly useful for avoiding namespace conflicts and further shortening type references. For instance, @using Galahad alias x permits the use of x:Type as a shorthand for types in the Galahad namespace.

- Attached Properties and Alias: If the namespace contains attached properties, you must explicitly use the alias when referencing these properties. For example, <Type x:Name="exampleName"/> is necessary for utilizing attached properties defined in the Galahad namespace (assuming x is the alias for Galahad).

Below is an example demonstrating the use of @using in a Galahad document:

```xml
@using Galahad alias x
@using Galahad.AbstractPresentation

<UserControl>
    <Grid RowDefinations="[*, Auto]">
        <!-- Example usage of Galahad with @using directive -->
    </Grid>
</UserControl>
```

### Defining Resources
The [key]="value" attribute in Galahad is used for defining resources and component properties. It is crucial to explicitly specify the data types for both key and value. This ensures clarity and precision in defining properties and their values, making the code more readable and maintainable. For instance, in the attribute ["FontSize"] = "double(14)", double explicitly states the data type for the value 14.

Galahad is designed to provide a more compact and expressive representation of structures that in XAML might require significantly more space. This approach allows developers to more quickly and easily create complex user interfaces, leveraging the power and flexibility of the Galahad language.
