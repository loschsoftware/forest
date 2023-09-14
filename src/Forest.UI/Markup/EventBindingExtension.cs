using Forest;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;

namespace Losch.Installer.Markup;

/// <summary>
/// Allows binding a method to an event inside of an installer page.
/// </summary>
public class EventBindingExtension : MarkupExtension
{
    /// <summary>
    /// Creates a new instance of the <see cref="EventBindingExtension"/> type with the specified handler name.
    /// </summary>
    /// <param name="handler">The event handler method.</param>
    public EventBindingExtension(string handler) => Handler = handler;
    /// <summary>
    /// Creates a new instance of the <see cref="EventBindingExtension"/> type.
    /// </summary>
    public EventBindingExtension() { }

    /// <summary>
    /// The event handler method to bind to.
    /// </summary>
    [ConstructorArgument("handler")]
    public string Handler { get; set; }

    /// <summary>
    /// Evaluates the handler.
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns>The method to bind the event to.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (string.IsNullOrEmpty(Handler))
            throw new InvalidOperationException();

        IProvideValueTarget target = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
        EventInfo eventInfo = target.TargetProperty as EventInfo ?? throw new InvalidOperationException();

        object handler = GetHandler(eventInfo, Handler);
        return handler ?? throw new InvalidOperationException();
    }

    private object GetHandler(EventInfo eventInfo, string eventHandlerName)
    {
        static Type[] GetParameterTypes(EventInfo eventInfo)
        {
            var invokeMethod = eventInfo.EventHandlerType.GetMethod("Invoke");
            return invokeMethod.GetParameters().Select(p => p.ParameterType).ToArray();
        }

        foreach (object container in Context.Current.EventContainers)
        {
            Type dcType = container.GetType();

            MethodInfo method = dcType.GetMethod(
                eventHandlerName,
                GetParameterTypes(eventInfo));

            if (method != null)
            {
                if (method.IsStatic)
                    return Delegate.CreateDelegate(eventInfo.EventHandlerType, method);
                else
                    return Delegate.CreateDelegate(eventInfo.EventHandlerType, container, method);
            }
        }

        return null;
    }
}