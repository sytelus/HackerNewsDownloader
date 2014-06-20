require(plyr)
ppm <- read.table(file="c:\\temp\\HNPostTimeStats.TSV", sep="\t", header=T, stringsAsFactors = F)

is.nonEmpty <- function(s) is.null(s) || is.na(s) || s==""
csvToNumVect <- function(val) 
{
  safeVal <- ifelse(is.nonEmpty(val), "0", val)
  strVect <- unlist(strsplit(safeVal, ",", fixed=T))
  as.numeric(strVect)
}

adply(ppm, 1, transform, CL = csvToNumVect(Comments), PL = csvToNumVect(Points))
ppm$Points <- NULL
ppm$Comments <- NULL

head(ppm$PL)
