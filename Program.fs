open System
open System.Runtime.InteropServices
open System.Threading

[<Struct>]
type POINT =
    struct
        val mutable X: int
        val mutable Y: int
    end

[<DllImport("user32.dll")>]
extern bool GetCursorPos([<Out>] POINT& lpPoint)

[<DllImport("user32.dll")>]
extern bool SetCursorPos(int x, int y)

[<DllImport("user32.dll")>]
extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo)

let KEYEVENTF_KEYDOWN = 0u
let KEYEVENTF_KEYUP = 2u
let VK_F13 = 0x7Cu

let pressKey () =
    keybd_event (byte VK_F13, 0uy, KEYEVENTF_KEYDOWN, 0u)
    Thread.Sleep(100) // Small delay before releasing the key
    keybd_event (byte VK_F13, 0uy, KEYEVENTF_KEYUP, 0u)

let random = Random()

let moveMouse () =
    let mutable currentPos = POINT()

    if GetCursorPos(&currentPos) then
        let originalX = currentPos.X
        let originalY = currentPos.Y

        let direction = random.Next(4)

        let newX, newY =
            match direction with
            | 0 -> originalX + 2, originalY
            | 1 -> (max 0 (originalX - 2)), originalY
            | 2 -> originalX, originalY + 2
            | _ -> originalX, (max 0 (originalY - 2))

        SetCursorPos(newX, newY) |> ignore
        pressKey () // Simulate F13 key press

let jiggleMouse () =
    while true do
        moveMouse ()
        Thread.Sleep(30000)

[<EntryPoint>]
let main argv =
    printfn "Program is running. Press Ctrl+C to stop."
    jiggleMouse ()
    0
