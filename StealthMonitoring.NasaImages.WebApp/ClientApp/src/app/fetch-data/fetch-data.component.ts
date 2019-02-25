import {Component, Inject, OnDestroy, OnInit, ViewChild} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {GALLERY_CONF, GALLERY_IMAGE, NgxImageGalleryComponent} from 'ngx-image-gallery';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent implements OnInit, OnDestroy {
  public date: Date;
  private sub: any;
  private count;
  @ViewChild('ngxImageGallery') ngxImageGallery: NgxImageGalleryComponent;

  conf: GALLERY_CONF = {
    imageOffset: '0px',
    inline: true,
    backdropColor: 'white',
    showCloseControl: false,
    showExtUrlControl: false,
    showDeleteControl: false,
    showImageTitle: true,
    showThumbnails: false,
    showArrows: true
  };

  images: GALLERY_IMAGE[] = [

  ];

  constructor(private route: ActivatedRoute, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {

  }

  public ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
       this.date = params['date'];

       if (this.date != null) {
         this.images = [];
         this.count = null;

         this.http.get<ImageInfo[]>(this.baseUrl + 'api/images?date=' + this.date).subscribe(result => {
           this.count = result.length;

           for (let i = 0; i < result.length; i++) {
             this.images.push(<GALLERY_IMAGE>{
               title: result[i].name,
               altText: '',
               extUrl: '',
               thumbnailUrl: '',
               url: this.baseUrl + 'api/images/' + result[i].id
             });
           }

           this.ngxImageGallery.conf = this.conf;
           this.ngxImageGallery.images = this.images;
           this.ngxImageGallery.open(0);
         }, error => console.error(error));
       }
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}

interface ImageInfo {
  id: string;
  date: Date;
  category: string;
  name: string;
}

