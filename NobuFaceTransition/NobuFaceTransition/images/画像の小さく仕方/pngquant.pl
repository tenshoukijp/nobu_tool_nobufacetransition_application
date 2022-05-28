foreach my $png (<*.png>) {
	`pngquant --ext .png $png --force --speed 1`;
}