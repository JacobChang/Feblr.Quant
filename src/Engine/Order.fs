module Feblr.Quant.Engine.Order

open System

type Purpose =
    | Buy
    | Sell

type Strategy =
    | GoodTillCancel
    | GoodTillDate of DateTime
    | ImmediateOrCancel
    | FillOrKill
    | PriceLimit of Double
    | MarketPrice
    | StopLossPrice

type Order =
    { purpose: Purpose
      units: int64
      strategies: Strategy list
      timestamp: DateTime }

type OrderBook =
    { sells: Order seq
      buys: Order seq }

let matching() = ignore
