module Feblr.Quant.Engine.Account

open System.Net.Http
open FSharp.Control.Tasks.V2

type Account<'a> =
  { accountId: 'a
    totalBalance: double
    unrealisedPNL: double
    realisedPNL: double
    marginRate: double
    marginUsed: double
    marginAvailable: double
    netAssetValue: double
    openTrades: uint64
    currency: string
    amountAvailableRatio: double }

  static member create accountId currency totalBalance marginAvailable marginRate =
    { accountId = accountId
      currency = currency
      totalBalance = totalBalance
      marginRate = marginRate
      marginAvailable = marginAvailable
      marginUsed = 0.0
      unrealisedPNL = 0.0
      realisedPNL = 0.0
      openTrades = 0UL
      amountAvailableRatio = marginAvailable / totalBalance
      netAssetValue = marginAvailable }
