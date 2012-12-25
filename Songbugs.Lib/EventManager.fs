namespace Songbugs.Lib
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Input
open Songbugs.Lib.MiscOps
open Songbugs.Lib.Input

module EventManager =
  
  let mutable private oldKeybState = Keyboard.GetState ()
  let mutable private oldMouseState = Mouse.GetState ()
  
  let private keyPress = new Event<Keys>()
  let private keyDown = new Event<Keys>()
  let private keyRelease = new Event<Keys>()
  let private mousePress = new Event<MouseButtons>()
  let private mouseDown = new Event<MouseButtons>()
  let private mouseRelease = new Event<MouseButtons>()
  
  let KeyPress = keyPress.Publish
  let KeyDown = keyDown.Publish
  let KeyRelease = keyRelease.Publish
  let MousePress = mousePress.Publish
  let MouseDown = mouseDown.Publish
  let MouseRelease = mouseRelease.Publish
  
  // If the given key has been pressed, fire a KeyPress event
  let keyPressAction (curr : KeyboardState) (old : KeyboardState) b evArg (ev : _ Event) =
    if (curr.IsKeyDown b) && (old.IsKeyUp b) then
      ev.Trigger evArg
  
  // If the given key is down, fire a KeyDown event
  let keyDownAction (curr : KeyboardState) b evArg (ev : _ Event) =
    if curr.IsKeyDown b then
      ev.Trigger evArg
  
  // If the given key is up, fire a KeyUp event
  let keyReleaseAction (curr : KeyboardState) (old : KeyboardState) b evArg (ev : _ Event) =
    if (curr.IsKeyUp b) && (old.IsKeyDown b) then
      ev.Trigger evArg
  
  // If the given mouse button has been clicked, fire a MousePress event
  let mouseButtonPressAction curr old evArg (ev : _ Event) =
    if (curr = ButtonState.Pressed) && (old = ButtonState.Released) then
      ev.Trigger evArg
  
  // If the given mouse button is down, fire a MouseDown event
  let mouseButtonDownAction b evArg (ev : _ Event) =
    if b = ButtonState.Pressed then
      ev.Trigger evArg
  
  // If the given mosue button has been released (clicked), fire a MouseRelease event
  let mouseButtonReleaseAction curr old evArg (ev : _ Event) =
    if (curr = ButtonState.Released) && (old = ButtonState.Released) then
     ev.Trigger evArg
  
  let updateButton currb oldb b evPress evDown evRelease =
    mouseButtonPressAction currb oldb b evPress
    mouseButtonDownAction currb b evDown
    mouseButtonReleaseAction currb oldb b evRelease
  
  let updateMouseEvents () =
    let mouseState = Mouse.GetState ()
    let updateMouseButton currmb oldmb mb = updateButton currmb oldmb mb mousePress mouseDown mouseRelease
    
    for mb in ["left"; "middle"; "right"] do
      let short = (cap mb)
      let long = short + "Button"
      updateMouseButton (unbox ((?) mouseState long ())) (unbox ((?) oldMouseState long ())) (getEnumValue<MouseButtons> short)
    
    updateMouseButton mouseState.XButton1 oldMouseState.XButton1 MouseButtons.X1
    updateMouseButton mouseState.XButton2 oldMouseState.XButton2 MouseButtons.X2
    
    oldMouseState <- mouseState
  
  // Keep those events flowing.
  let update () =
    updateMouseEvents ()