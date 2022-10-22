module App

open System
open Feliz
open Elmish
open Fable.Core
open Elmish.React
open System.Text.RegularExpressions

type Item ={
    Age: int
    Name: string
}

let items = [
    {Age = 26; Name = "John"}
    {Age = 16; Name = "Bobby"}
    {Age = 32; Name = "Ann"}
]

type State = {
    FilterBox: string
    MainList: Item list
    NewList: Item list}

type Msg =
    |Filter of string


    
let init() = {
    FilterBox = ""
    MainList = items
    NewList = items}, Cmd.none

let update msg state  =
    match msg with

    |Filter text -> 
             let model = {state with FilterBox = text}
             let mainList = model.MainList
 //            let f = fun (s : Item) -> s.  (text)
             let f (s : Item) =
                 match s with
                 | s when s.Name.Contains(text) -> true
                 | s when (string s.Age).Contains(text) -> true
                 | _ -> false
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
            prop.children [
                Html.div [
                    prop.text item.Name
                ]
                Html.div [
                    prop.text item.Age
                ]
            ]
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