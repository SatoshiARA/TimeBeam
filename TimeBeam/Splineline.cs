﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TimeBeam.Helper;
using TimeBeam.Surrogates;

namespace TimeBeam {
  public partial class Splineline : TimelineBase {
    #region Constructor
    /// <summary>
    ///   Construct a new timeline.
    /// </summary>
    public Splineline() {
      InitializeComponent();
    }
    #endregion

    /// <summary>
    ///   Draw a list of splines onto the timeline.
    /// </summary>
    /// <param name="tracks">The splines to draw.</param>
    protected override void DrawTracks( IEnumerable<ITrackSegment> tracks, Graphics graphics ) {
      Rectangle trackAreaBounds = GetTrackAreaBounds();
      
      // Grab only the spline segments. All passed tracks should be spline segments, but just make sure anyway.
      // Also store them in a list to avoid multiple iterations over enumerable.
      List<ISplineSegment> splineSegments = tracks.OfType<ISplineSegment>().ToList();

      // Generate colors for the tracks.
      List<Color> colors = ColorHelper.GetRandomColors( splineSegments.Count() );

      foreach( ISplineSegment splineSegment in splineSegments ) {
        // The extent of the track segment, including the border.
        RectangleF trackExtent = BoundsHelper.GetTrackExtents( splineSegment, this );

        // Don't draw track segments that aren't within the target area.
        if( !trackAreaBounds.IntersectsWith( trackExtent.ToRectangle() ) ) {
          continue;
        }

        // The index of this track segment (or the one it's a substitute for).
        int trackIndex = TrackIndexForTrack( splineSegment );

        // Determine colors for this track segment.
        Color trackColor = ColorHelper.AdjustColor( colors[ trackIndex ], 0, -0.1, -0.2 );

        if( _selectedTracks.Contains( splineSegment ) ) {
          trackColor = Color.WhiteSmoke;
        }

        // Draw the main track segment area.
        if( splineSegment is TrackSegmentSurrogate ) {
          // Draw surrogates with a transparent brush.
          graphics.DrawLine( new Pen(Color.FromArgb( 128, trackColor ) ), trackExtent.Left, trackExtent.Bottom, trackExtent.Right, trackExtent.Top );
        } else {
          graphics.DrawLine( new Pen(trackColor ), trackExtent.Left, trackExtent.Bottom, trackExtent.Right, trackExtent.Top );
        }
      }
    }
  }
}
