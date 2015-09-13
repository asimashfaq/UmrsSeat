function drawchart(data)
{
    $("#svgContent,#testtable").html('');
    
    width = 350; // Changes pie size as a whole
    height = 326; // Changes pie size as a whole
    radius = 150;

    var color = d3.scale.ordinal()
    .range(['#6D5CAE', '#E2DEEF', '#48B0F7', '#6DC0F9']);

    var arc = d3.svg.arc()  //Size of donut chart 
        .outerRadius(radius)
        .innerRadius(80); //Changes width of the slices of the pie

    var arcOver = d3.svg.arc()  // Size of donut chart when hovering
        .outerRadius(radius + 5)
        .innerRadius(85);

    var svg = d3.select("#svgContent").append("svg")
        .attr("width", width)
        .attr("height", height)
        .append("g")
        .attr("transform", "translate(" + 170 + "," + radius * 1.1 + ")");
    div = d3.select("body")
    .append("div")
    .attr("class", "tooltip");
    var oldPieData = [];
    var pie = d3.layout.pie()
          .sort(null)
         .startAngle(-Math.PI / 2)
            .endAngle(Math.PI + Math.PI / 2)
          .value(function (d) { return d.noOfSeats; });

    var g = svg.selectAll(".arc")
        .data(pie(data))
        .enter()
            .append("g")
                .attr("class", "arc")
                .on("mousemove", function (d) {
                    var mouseVal = d3.mouse(this);
                    div.style("display", "none");
                    div.html( "Value:" + d.data.noOfSeats + "</br>Type:" + d.data.Type)
                    .style("left", (d3.event.pageX + 12) + "px")
                    .style("top", (d3.event.pageY - 10) + "px")
                    .style("opacity", 1)
                    .style("display", "block");
                })
                .on("mouseover", function (d) {
                    d3.select(this).select("path").transition()
                        .duration(100)
                        .attr("d", arcOver);
                })
                .on("mouseout", function (d) {
                    div.html(" ").style("display", "none");
                    d3.select(this).select("path").transition()
                       .duration(100)
                       .attr("d", arc);
                });

 
    var paths = g.append("path")

        .attr("data-legend", function (d) { return d.data.Type })
        .attr('d', arc)
          .attr('fill', function (d, i) {
              return color(d.data.Type);
          }).transition()
                .duration(500)
                .attrTween("d", pieTween);




    function pieTween(d, i) {
        var s0;
        var e0;

        if (oldPieData[i]) {
            s0 = oldPieData[i].startAngle;
            e0 = oldPieData[i].endAngle;
        } else if (!(oldPieData[i]) && oldPieData[i - 1]) {
            s0 = oldPieData[i - 1].endAngle;
            e0 = oldPieData[i - 1].endAngle;
        } else if (!(oldPieData[i - 1]) && oldPieData.length > 0) {
            s0 = oldPieData[oldPieData.length - 1].endAngle;
            e0 = oldPieData[oldPieData.length - 1].endAngle;
        } else {
            s0 = 0;
            e0 = 0;
        }

        var i = d3.interpolate({ startAngle: s0, endAngle: e0 }, { startAngle: d.startAngle, endAngle: d.endAngle });
        return function (t) {
            var b = i(t);
            return arc(b);
        };
    }






    var keys = [  "Type","noOfSeats"];
    var table = d3.select("#testtable");
    var thead = table.append("thead");
      var  tbody = table.append("tbody");
        thead
        .append("tr")
        .selectAll(".head")
        .data(keys)
        .enter()
        .append("th")
        .attr("class", "head")
        .text(function (d) { return d; });

    tbody
        .selectAll(".dataRow1")
        .data(data)
        .enter()
        .append("tr")
        .attr("class", "dataRow1")
        .on("mouseover", function (d, i) {
            console.log(i);
            var path = paths[0][i];
            d3.select(path).transition()
                        .duration(100)
                        .attr("d", arcOver);
        })
        .on("mouseout", function (d, i) {
      
            var path = paths[0][i];
            d3.select(path).transition()
            .duration(100)
            .attr("d", arc);
        });


    d3.selectAll("#testtable .dataRow1")
        .selectAll("td")
        .data(function (row) {
            return keys.map(function (d) {
                return { value: row[d] };
            });
        })
        .enter()
        .append("td")
        .html(function (d) {
          /*  if (d.value == "Seats Avaliable	") {
                return "<span class='as'></span>" + d.value;
            }
            else if (d.value == "Group Split") {
                return "<span class='gs'></span>" + d.value;
            }
            else if (d.value == "Stock Selling") {
                return "<span class='ss'></span>" + d.value;
            }
            else if (d.value == "Stock Transfer") {
                return "<span class='ts'></span>" + d.value;
            }*/
            
            return d.value;
        });
 /*   $(".as").parent().addClass('tas');
    $(".gs").parent().addClass('tgs');
    $(".ss").parent().addClass('tss');
    $(".ts").parent().addClass('tts');*/
    $("#ginfo").addClass('col-md-6').removeClass('col-md-12');
    $("#pinfo").show();
}