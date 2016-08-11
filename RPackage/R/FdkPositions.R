#' Gets the account positions
#' 
ttTrade_Position <- function(){
  symInfo = GetTradePositions()
  
  agentComission = GetPositionAgentCommission(symInfo)
  buyAmount = GetPositionBuyAmount(symInfo)
  buyPrice = GetPositionBuyPrice(symInfo)
  comission = GetPositionCommission(symInfo)
  profit = GetPositionProfit(symInfo)
  sellAmount = GetPositionSellAmount(symInfo)
  sellPrice = GetPositionSellPrice(symInfo)
  settlementPrice = GetPositionSettlementPrice(symInfo)
  swap = GetPositionSwap(symInfo)
  symbol = GetPositionSymbol(symInfo)
  
  UnregisterVar(symInfo)
  
  data.table(agentComission, buyAmount, buyPrice, comission, profit,
    sellAmount, sellPrice, settlementPrice, swap, symbol
  )
}
#' Get trade history
GetTradePositions <- function() {
  rClr::clrCallStatic('FdkRHost.FdkPosition', 'GetTradePositions')
}

#' Get trade comission
#' @param symInfo RHost variable that stores the array
GetPositionAgentCommission <- function(symInfo) {
  rClr::clrCallStatic('FdkRHost.FdkPosition', 'GetPositionAgentCommission', symInfo)
}

#' Get trade comission
#' @param symInfo RHost variable that stores the array
GetPositionBuyAmount <- function(symInfo) {
  rClr::clrCallStatic('FdkRHost.FdkPosition', 'GetPositionBuyAmount', symInfo)
}

#' Get trade comission
#' @param symInfo RHost variable that stores the array
GetPositionBuyPrice <- function(symInfo) {
  rClr::clrCallStatic('FdkRHost.FdkPosition', 'GetPositionBuyPrice', symInfo)
}

#' Get trade comission
#' @param symInfo RHost variable that stores the array
GetPositionCommission <- function(symInfo) {
  rClr::clrCallStatic('FdkRHost.FdkPosition', 'GetPositionCommission', symInfo)
}

#' Get trade comission
#' @param symInfo RHost variable that stores the array
GetPositionProfit <- function(symInfo) {
  rClr::clrCallStatic('FdkRHost.FdkPosition', 'GetPositionProfit', symInfo)
}

#' Get trade comission
#' @param symInfo RHost variable that stores the array
GetPositionSellAmount <- function(symInfo) {
  rClr::clrCallStatic('FdkRHost.FdkPosition', 'GetPositionSellAmount', symInfo)
}

#' Get trade comission
#' @param symInfo RHost variable that stores the array
GetPositionSellPrice <- function(symInfo) {
  rClr::clrCallStatic('FdkRHost.FdkPosition', 'GetPositionSellPrice', symInfo)
}

#' Get trade comission
#' @param symInfo RHost variable that stores the array
GetPositionSettlementPrice <- function(symInfo) {
  rClr::clrCallStatic('FdkRHost.FdkPosition', 'GetPositionSettlementPrice', symInfo)
}

#' Get trade comission
#' @param symInfo RHost variable that stores the array
GetPositionSwap <- function(symInfo) {
  rClr::clrCallStatic('FdkRHost.FdkPosition', 'GetPositionSwap', symInfo)
}

#' Get trade comission
#' @param symInfo RHost variable that stores the array
GetPositionSymbol <- function(symInfo) {
  rClr::clrCallStatic('FdkRHost.FdkPosition', 'GetPositionSymbol', symInfo)
}
