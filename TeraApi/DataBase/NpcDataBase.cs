using Detrav.TeraApi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detrav.TeraApi.DataBase
{
    public class NpcDataBase
    {

        public ushort header;
        public uint id;
        public string name;
        public ushort level;
        public NpcType type;
        public uint hp;
        public string location;
        public ulong ulongId { get { return ((ulong)header << 32) + id; } }
        /*
         * На сайте выложили базу, зачем напрягаться с дешифровкой игры когда можно взять у них:
         * http://teradatabase.net/
         * http://teradatabase.net/ru/npcs/monsters/
var myArray = [];
$('#NpcTable tbody tr').each(function(a){ 
var el = {}
el['id'] = $(this).find(' .dt-id').text();
el['name'] = $(this).find(' .dt-title b').text();
el['level'] = $($(this).find(' .dt-level')[0]).text();
el['hp'] = $($(this).find(' .dt-level')[1]).text();
console.log(el);
myArray.push(el);
});
var result = "";
$('#NpcTable tbody tr').each(function(a){ 
result += "id:"+ $(this).find(' .dt-id').text() + ", ";
result += "name:"+  $(this).find(' .dt-title b').text() + ", ";
result += "level:"+ $($(this).find(' .dt-level')[0]).text() + ", ";
result += "hp:"+ $($(this).find(' .dt-level')[1]).text() + "\n";
});
var result = "";
var ii1 = 0;
function addToResult() {$('#NpcTable tbody tr').each(function(a)
result += '{"id":"'+ $(this).find(' .dt-id').text() + '", ';
result += '"name":"'+  $(this).find(' .dt-title b').text() + '", ';
result += '"level":"'+ $($(this).find(' .dt-level')[0]).text() + '", ';
result += '"hp":"'+ $($(this).find(' .dt-level')[1]).text() + '"},\n';
ii1++;
if(ii1<53) setTimeout(addToResult(),2000);
});
$($('.next.paginate_button')[0]).click();};
for (var i = 0; i < 53; i++) {
{

}
         */

        /*
var result = "";
var ii1 = 0;
function grs(a)
{ return a.replace(/"/g,"\\\"");}
function addToResult() {
    $('#NpcTable tbody tr').each(
        function(a) {
            result += "{\"id\":\"" + $(this).find(" .dt-id").text() + "\", ";
            result += "\"name\":\"" + grs($(this).find(" .dt-title b").text()) + "\", ";
            result += "\"level\":\"" + $($(this).find(" .dt-level")[0]).text() + "\", ";
            result += "\"hp\":\"" + $($(this).find(" .dt-level")[1]).text() + "\"},\n";
        });
    ii1++;
    $($('.next.paginate_button')[0]).click();
    if (ii1 < 53) setTimeout(addToResult, 100);
}
addToResult()
$("body").text(result);
         */
    }
   
}
