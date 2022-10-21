module App

open System
open Feliz
open Elmish
open Fable.Core
open Elmish.React
open System.Text.RegularExpressions



type State = {
    FilterBox: string
    MainList: string list
    NewList: string list}

type Msg =
    |Filter of string


    
let init() = {
    FilterBox = ""
    MainList = ["first"; "Second"; "Se"]
    NewList =[]}, Cmd.none

let update msg state  =
    match msg with

    |Filter text -> 
             let model = {state with FilterBox = text}
             let mainList = model.MainList
             let f = fun (s : string) -> s.Contains(text)
             let newList = mainList|>List.filter f
             let nextState = {model with NewList = newList}
             nextState, Cmd.none
    |Filter text when state.FilterBox = "" -> {state with NewList = state.MainList}, Cmd.none
        

            

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

let filteredList (state: State) (dispatch: Msg -> unit) =
  Html.ul [
    prop.children [
      for item in state.NewList ->
        Html.li [
          prop.classes ["box"; "subtitle"]
          prop.text item
        ]
    ]
  ]



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
        filteredList state dispatch
      ]
    ]
  



Program.mkProgram init update render
|> Program.withConsoleTrace
|> Program.withReactSynchronous "elmish-app"
|> Program.run