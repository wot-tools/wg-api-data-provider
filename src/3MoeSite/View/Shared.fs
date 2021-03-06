﻿[<AutoOpen>]
module _3MoeSite.Views.Shared
open Giraffe.GiraffeViewEngine
open _3MoeSite.Model
open System
open System.Linq
open WGApiDataProvider
open _3MoeSite.Views
open _3MoeSite



type StatValueObject =
    | I of int
    | D of decimal
    | F of float

let printStatValueObject (o : StatValueObject) =
    match o with
    | I i -> System.String.Format("{0:N0}", i)
    | D d -> System.String.Format("{0:P2}", d)
    | F f -> System.String.Format("{0:N0}", f)

let headlineBlock headerText = 
    div [ _class "headline" ] [
        encodedText headerText
    ]

let errorBlock errorText = 
    [
        div [ _class "errorBox"] [
            span [] [ encodedText "Error" ]
            br []
            encodedText errorText
        ]
    ] |> layout "Error!"

let linkWithImage (link : string) (title: string) (imgSrc : string) = 
    a [ _href link
        _target "blank" 
        _class "linkWithImage"] [
            img [ _src imgSrc ]
            encodedText title
        ]

let dateBlock (displayValue : DateTime) text = 
    div [ _class "dateValueBox" ] [
            div [ _class "dateValueBoxDiv" ] [ encodedText (System.String.Format("{0:g}", displayValue)) ]
            encodedText text
        ]

let clanLinkWithIcon (clan : Clan) = 
    match clan.ID with
    | 0 -> span [] []
    | _ -> a [ _href (Links.clanPage clan.ID)
               _target "blank" 
               _class "clanTagLinkWithIcon"] [
                    img [ _src clan.Emblems.x24.Portal ]
                    encodedText clan.Tag
           ]

let tankDisplayTable = customTable ([
    createColumn "Contour" "contour" (fun t -> Img t.Icons.Contour)
    createCustomColumn "Name"         "name"  (fun t -> S t.Name)
        (fun t -> (a [ _href (Links.tankPage t.ID) ] [
                        encodedText t.Name ]))
    createColumn "Short Name"   "sn"    (fun t -> S t.ShortName)
    createColumn "3 MoE"        "moe"   (fun t -> TableCellObject.I t.ThreeMoeCount)
    createColumn "MoE Value"    "moev"  (fun t -> TableCellObject.D t.MoeValue)
    createCustomColumn "Tier"         "tier"  (fun t -> TableCellObject.I t.Tier)
        (fun t -> (linkWithImage (Links.tierPage t.Tier) (string t.Tier) ""))
    createCustomColumn "Nation"       "nat"   (fun t -> E t.Nation)
        (fun t -> (linkWithImage (Links.nationPage (int t.Nation)) (string t.Nation) (Links.nationFlag ((string t.Nation).ToLower()))))
    createCustomColumn "Type"         "type"  (fun t -> E t.VehicleType)
        (fun t -> (linkWithImage (Links.vehicleTypePage (int t.VehicleType)) (string t.VehicleType) ""))
    ] : Tank Column List)
