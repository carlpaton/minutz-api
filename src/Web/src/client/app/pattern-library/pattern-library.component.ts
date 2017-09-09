import { Component, AfterViewInit, ViewEncapsulation } from '@angular/core';
declare let $:any;

/**
 * This class represents the lazy loaded PatternLibraryComponent.
 */
@Component({
  moduleId: module.id,
  selector: 'sd-pattern-library',
  templateUrl: 'pattern-library.component.html',
  styleUrls: ['pattern-library.component.css'],
  encapsulation: ViewEncapsulation.None  // Use to disable CSS Encapsulation for this component
})
export class PatternLibraryComponent implements AfterViewInit {

  ngAfterViewInit() {

    $('#pattern-library').on('click', 'a[href="#"]', function(event:any) {
        event.preventDefault();
    });


    var $button = $('<span id="source-button" class="js-source-button btn btn-primary btn-xs">&lt; &gt;</span>').click(function() {
        var html = $(this).parent().html();
        html = cleanSource(html);
        $('#source-modal pre').text(html);
        $('#source-modal').modal();
    });

    $('.bs-component [data-toggle="popover"]').popover();
    $('.bs-component [data-toggle="tooltip"]').tooltip();


    $('#pattern-library').on('mouseenter', '.bs-component', function() {
        $(this).append($button);
        $button.show();
    });
    $('#pattern-library').on('mouseleave', '.bs-component', function() {
        $button.hide();
    });


    function cleanSource(html:any) {
        html = html.replace(/×/g, '&times;')
            .replace(/«/g, '&laquo;')
            .replace(/»/g, '&raquo;')
            .replace(/←/g, '&larr;')
            .replace(/→/g, '&rarr;');

        var lines = html.split(/\n/);

        lines.shift();
        lines.splice(-1, 1);

        var indentSize = lines[0].length - lines[0].trim().length,
            re = new RegExp('{" + indentSize + "}');

        lines = lines.map(function(line:any) {
            if (line.match(re)) {
                line = line.substring(indentSize);
            }

            return line;
        });

        lines = lines.join('\n');

        return lines;
    }
  }

}
