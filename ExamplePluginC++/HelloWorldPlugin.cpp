#include <string>

using namespace System;
using namespace Avalonia::Controls;
using namespace CassieLauncher::Plugins;


class NativeGreeter
{
public:
    std::string Greet() const { return "Hello from native C++"; }
};

namespace HelloWorldCpp
{
    public ref class HelloWorldPlugin : public IFnPlugin
    {
    private:
        NativeGreeter* _native;

    public:
        HelloWorldPlugin()
        {
            _native = new NativeGreeter();
        }

        !HelloWorldPlugin()
        {
            delete _native;
        }

        ~HelloWorldPlugin()
        {
            this->!HelloWorldPlugin();
        }

        virtual property String^ Name { String^ get() { return "Hello World (C++)"; } }
        virtual property String^ Version { String^ get() { return "1.0.0"; } }
        virtual property String^ Author { String^ get() { return "YourName"; } }
        virtual property String^ Description { String^ get() { return "Sample C++/CLI plugin."; } }

        virtual Control^ CreateView()
        {
            std::string msg = _native->Greet();
            String^ managedMsg = gcnew String(msg.c_str());

            auto text = gcnew TextBlock();
            text->Text = managedMsg;
            text->Margin = Avalonia::Thickness(12);
            return text;
        }
    };
}