import { ViewEngineHooks, View } from 'aurelia-framework';
// import { viewEngineHooks } from 'aurelia-binding';
import { MessageStatus } from './globals';

// By convention, Aurelia will look for any classes of the form 
// {name}ViewEngineHooks and load them as a ViewEngineHooks resource. We can
// use the @viewEngineHooks decorator instead if we want to give the class a
// different name.
export class MessageStatusViewEngineHooks implements ViewEngineHooks {
  
  // The `beforeBind` method is called before the ViewModel is bound to
  // the view. We want to expose the enum to the binding context so that
  // when Aurelia binds the data it will find our MediaType enum.
  beforeBind(view: View) {

    // We add the enum to the override context. This will expose the enum
    // to the view without interfering with any properties on the
    // bindingContext itself.
    view.overrideContext['MessageStatus'] = MessageStatus;

    // Since TypeScript enums are not iterable, we need to do a bit of extra
    // legwork if we plan on iterating over the enum keys.
    // view.overrideContext['MessageStatus'] = 
    //   Object.keys(MessageStatus)
    //     .filter((key) => typeof MessageStatus[key] === 'number');
  }
}