module Feblr.Quant.Engine.Instrument

open System

type CandleStickPriceType =
  | Mid
  | Bid
  | Ask

type CandleStick =
  { time: DateTime
    openPrice: double
    closePrice: double
    highestPrice: double
    lowestPrice: double }

type CandleStickGranularity =
  | Seconds of int
  | Minutes of int
  | Hours of int
  | Days of int
  | Months of int

  member this.toSeconds () =
    match this with
    | Seconds seconds -> seconds
    | Minutes minutes -> minutes * 60
    | Hours hours -> hours * 60 * 60
    | Days days -> days * 24 * 60 * 60
    | Months months -> months * 30 * 24 * 60  * 60

type InstrumentPairInterestRate =
  { baseCurrencyBid: double
    baseCurrencyAsk: double
    quoteCurrencyBid: double
    quoteCurrencyAsk: double }

type Instrument<'a> =
  { id: 'a
    name: string
    description: string
    pip: double
    interestRate: InstrumentPairInterestRate }
