# coding: ASCII-8BIT
# Needed to concatenate strings to data in Excel

require 'rubygems'
require 'parseexcel'
require 'iconv'

CD = Iconv.new('utf-8', 'iso-8859-1')
def utfprint(fp, data)
  fp.print(CD.iconv(data))
end


TYP=0
LATITUD=1
LONGITUD=2
ETIKETT_1=3
ETIKETT_2=4
ETIKETT_3=5
ETIKETT_4=6
BYGGNAD_SV=7
BYGGNAD_EN=8
VANING=9
STAD=10
RUMSTYP_SV=11
RUMSTYP_EN=12

TYP_ROOM="0"
TYP_BUILDING="1"


File.open(ARGV[ARGV.length-1], "w") { |fp|

  (0..(ARGV.length-2)).each { |param|

    puts ARGV[param]
    workbook = Spreadsheet::ParseExcel.parse(ARGV[param])
    worksheet = workbook.worksheet(0)

    rowNo = 1
    worksheet.each { |row|
      rowNo += 1
      if row != nil
        cells = row.collect { |cell| cell == nil ? "" : cell.to_s('latin1') }
        if cells[TYP] == "0" || cells[TYP] == "1" then
          utfprint fp, cells[TYP] + ";" + cells[LATITUD] + ";" + cells[LONGITUD] + "|"

          case cells[TYP]

            when TYP_ROOM
              sep = "";
              cells[ETIKETT_1..ETIKETT_4].each { |e|
                if e != "" then
                  utfprint fp, sep
                  sep = ";"
                  utfprint fp, e + ", " + cells[BYGGNAD_SV]
                  utfprint fp, " (" + cells[BYGGNAD_EN] + ")" unless cells[BYGGNAD_EN] == ""
                  utfprint fp, ", " + cells[STAD]
                end
              }

              utfprint fp, "|"
              utfprint fp, "Våning " + cells[VANING] + ", rum " + cells[ETIKETT_1]
              if cells[RUMSTYP_SV] != nil && cells[RUMSTYP_SV] != "" then
                utfprint fp, ". " + cells[RUMSTYP_SV] + ".;sv_SE"
              end

              utfprint fp, ";Floor " + cells[VANING] + ", room " + cells[ETIKETT_1]
              if cells[RUMSTYP_EN] != nil && cells[RUMSTYP_EN] != "" then
                utfprint fp, ". " + cells[RUMSTYP_EN] + ".;en_US"
              end

            when TYP_BUILDING
              utfprint fp, cells[BYGGNAD_SV]
              utfprint fp, " (" + cells[BYGGNAD_EN] + ")" unless cells[BYGGNAD_EN] == ""
              utfprint fp, ", " + cells[STAD]
              utfprint fp, "|"

              sep = "";
              if cells[RUMSTYP_SV] != nil && cells[RUMSTYP_SV] != "" then
                utfprint fp, cells[RUMSTYP_SV] + ";sv_SE"
                sep = ";"
              end

              if cells[RUMSTYP_EN] != nil && cells[RUMSTYP_EN] != "" then
                utfprint fp, sep + cells[RUMSTYP_EN] + ";en_US"
              end

          end

          utfprint fp, "\n"
        end
      end
    }
  }
}