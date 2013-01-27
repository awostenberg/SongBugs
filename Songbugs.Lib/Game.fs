namespace Songbugs.Lib
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Songbugs.Lib.Input

type Game() as this =
  inherit StateBasedGame ()
  
  let mutable screens : GameScreen list = []
  
  member this.Size = new Vector2(this.Graphics.PreferredBackBufferWidth |> float32, this.Graphics.PreferredBackBufferHeight |> float32)
  member val Graphics : GraphicsDeviceManager = new GraphicsDeviceManager(this) with get, set
  
  override this.Initialize () =
    this.Content.RootDirectory <- "../Resources/Media"
    this.Graphics.IsFullScreen <- false
    this.IsMouseVisible <- true
    screens <- [new MainMenu(this, 0, this.Size); new Board(this, 1)]
    List.iter (fun (screen : GameScreen) -> screen.Initialize ()) screens
    this.Window.AllowUserResizing <- true
    
    base.Initialize ()
  
  override this.LoadContent () = List.iter (fun (screen : GameScreen) -> screen.LoadContent ()) screens
  
  override this.Update gameTime =
    EventManager.update ()
    screens.[this.CurrentScreen].Update gameTime
  
  override this.Draw gameTime =
    this.GraphicsDevice.Clear Color.White
    screens.[this.CurrentScreen].Draw gameTime