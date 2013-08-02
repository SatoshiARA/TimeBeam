﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TimeBeam {
  /// <summary>
  /// The main host control.
  /// </summary>
  public partial class Timeline : UserControl {

    #region Layout
    /// <summary>
    ///   How high a single track should be.
    /// </summary>
    [Description( "How high a single track should be." )]
    [Category( "Layout" )]
    public int TrackHeight {
      get { return _trackHeight; }
      set { _trackHeight = value; }
    }

    private int _trackHeight = 20;

    /// <summary>
    /// How wide/high the border around a track item should be.
    /// This border allows you to interact with an item, even when it has zero size.
    /// </summary>
    public int TrackBorderSize {
      get { return _trackBorderSize; }
      set { _trackBorderSize = value; }
    }
    /// <summary>
    /// Backing field for <see cref="TrackBorderSize"/>.
    /// </summary>
    private int _trackBorderSize = 2;
    #endregion

    #region Drawing
    /// <summary>
    ///   The backbuffer itself.
    /// </summary>
    private Bitmap PixelMap { get; set; }

    /// <summary>
    ///   The <see cref="Graphics" /> object to draw into the backbuffer.
    /// </summary>
    protected Graphics GraphicsContainer { get; set; }

    /// <summary>
    ///   The background color of the timeline.
    /// </summary>
    [Description( "The background color of the timeline." )]
    [Category( "Drawing" )]
    public Color BackgroundColor {
      get { return _backgroundColor; }
      set { _backgroundColor = value; }
    }

    /// <summary>
    ///   Backing field for <see cref="BackgroundColor" />.
    /// </summary>
    private Color _backgroundColor = Color.Black;
    #endregion

    private List<ITimelineTrack> Tracks  = new List<ITimelineTrack>();


    /// <summary>
    ///   Construct a new timeline.
    /// </summary>
    public Timeline() {
      InitializeComponent();
      InitializePixelMap();
    }

    public void AddTrack( ITimelineTrack track ) {
      Tracks.Add( track );
      Redraw();
      Refresh();
    }

    /// <summary>
    ///   Redraws the timeline.
    /// </summary>
    /// <exception cref="NotImplementedException">Should be overridden in derived class.</exception>
    private void Redraw() {
      // Clear the buffer
      GraphicsContainer.Clear( BackgroundColor );

      int trackOffset = 0;
      foreach( ITimelineTrack track in Tracks ) {
        // The extent of the track, including the border
        RectangleF trackExtent = new RectangleF( track.Start - TrackBorderSize, trackOffset, track.End + TrackBorderSize, TrackHeight + TrackBorderSize * 2 );

        GraphicsContainer.FillRectangle( new SolidBrush( Color.DarkRed ), trackExtent );
        GraphicsContainer.DrawRectangle( new Pen( Color.Fuchsia,TrackBorderSize ), trackExtent.X, trackExtent.Y, trackExtent.Width, trackExtent.Height );

        // Offset the next track to the appropriate position.
        trackOffset += ( TrackBorderSize * 2 ) + TrackHeight;
      }
    }

    /// <summary>
    ///   Initialize the backbuffer
    /// </summary>
    private void InitializePixelMap() {
      PixelMap = new Bitmap( Width, Height );
      GraphicsContainer = Graphics.FromImage( PixelMap );
      GraphicsContainer.Clear( BackgroundColor );
      Refresh();
    }

    #region Event Handler
    /// <summary>
    ///   Invoked when the control is resized.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TimelineResize( object sender, EventArgs e ) {
      InitializePixelMap();
      Redraw();
      Refresh();
    }

    /// <summary>
    ///   Invoked when the control is repainted
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TimelinePaint( object sender, PaintEventArgs e ) {
      e.Graphics.DrawImage( PixelMap, 0, 0 );
    }

    /// <summary>
    /// Invoked once the timeline has loaded.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TimelineLoad( object sender, EventArgs e ) {
      Redraw();
      Refresh();
    }
    #endregion

  }
}