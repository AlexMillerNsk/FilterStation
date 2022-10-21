module App

open System
open Feliz
open Elmish
open Fable.Core
open Elmish.React

type Item = {
    id: int
    content: string}

let item1 = {
    id =1
    content = "First"}

let item2 = {
    id =2
    content = "Second"}

type Status =
    |Filtered
    |NoFiltered

type State = {
    FilterBox: string
    FilterItems: Item list
    Tester: string
    Status: Status}

type Msg =
    |SetFilterBox of string
    |Filter of string


    
let init() = {
    FilterBox = ""
    FilterItems = [item1; item2]
    Tester = "BusinessLogic"
    Status = NoFiltered}, Cmd.none

let update msg state  =
    match msg with
    |SetFilterBox x -> {state with FilterBox = x}, Cmd.none
    |Filter text -> 
        let model = {state with FilterBox = text}
        let itemMatch = model.Tester.Substring(0,model.FilterBox.Length);
        if model.FilterBox = itemMatch then
            { model with Status = Filtered}, Cmd.none
        else
            {model with Status = NoFiltered}, Cmd.none

let inputField (state: State) (dispatch: Msg -> unit) =
  Html.div [
    prop.classes [ "field"; "has-addons" ]
    prop.children [
      Html.div [
        prop.classes [ "control"; "is-expanded"]
        prop.children [
          Html.input [
            prop.classes [ "input"; "is-medium" ]
            prop.valueOrDefault state.FilterBox
            prop.onTextChange (Filter >> dispatch)
          ]
        ]
      ]
    ]
  ]

let renderText state dispatch =
    Html.div [
        prop.style [
            style.margin 30
            style.paddingLeft 10
            style.fontSize 20
        ]

        prop.text state.Tester
    ]

let renderNoText state dispatch =
    Html.div [
        prop.style [
            style.margin 30
            style.paddingLeft 10
            style.fontSize 20
        ]

        prop.text " Bad filter action"
    ]

let renderItem (state: State) (dispatch: Msg -> unit) =
    match state.Status with
    |Filtered -> renderText state dispatch
    |NoFiltered -> renderNoText state dispatch

let appTitle =
  Html.p [
    prop.className "title"
    prop.text "BusinessLogic"
  ]

let render state dispatch =
  Html.div [
    prop.style [ style.padding 20]
    prop.children [
        appTitle
        inputField state dispatch
        renderItem state dispatch
      ]
    ]
  



Program.mkProgram init update render
|> Program.withConsoleTrace
|> Program.withReactSynchronous "elmish-app"
|> Program.run